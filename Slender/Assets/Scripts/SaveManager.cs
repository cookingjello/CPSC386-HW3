using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveManager
{
    public static string SaveFilePath => Path.Combine(Application.persistentDataPath, "savegame.json");
    // inside SaveManager (anywhere near the top of the class)
public static bool LoadAfterSceneLoad = false;


    [System.Serializable]
    private class PaperEntry
    {
        public float x;
        public float y;
        public float z;
        public bool isActive;
    }

    [System.Serializable]
    private class SaveData
    {
        public float playerX;
        public float playerY;
        public float playerZ;
        public int numberOfPapers;
        public List<PaperEntry> papers = new List<PaperEntry>();
    }

    public static void SaveGame()
    {
        var playerInv = Object.FindObjectOfType<PlayerInventory>();
        if (playerInv == null)
        {
            Debug.LogError("Save failed: no PlayerInventory found in scene.");
            return;
        }

        var playerTransform = playerInv.transform;

        SaveData data = new SaveData();
        data.playerX = playerTransform.position.x;
        data.playerY = playerTransform.position.y;
        data.playerZ = playerTransform.position.z;
        data.numberOfPapers = playerInv.NumberOfPapers;

        // Find all Paper components in the current scene (including inactive)
        var allPapers = Resources.FindObjectsOfTypeAll<Paper>();
        var activeScene = SceneManager.GetActiveScene();
        foreach (var p in allPapers)
        {
            if (p == null || p.gameObject == null) continue;
            if (p.gameObject.scene != activeScene) continue; // skip prefabs/assets and other scenes

            PaperEntry pe = new PaperEntry();
            var pos = p.transform.position;
            pe.x = pos.x; pe.y = pos.y; pe.z = pos.z;
            pe.isActive = p.gameObject.activeSelf;
            data.papers.Add(pe);
        }

        string json = JsonUtility.ToJson(data, true);
        try
        {
            File.WriteAllText(SaveFilePath, json);
            Debug.Log("Saved game to: " + SaveFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to write save file: " + e.Message);
        }
    }

    public static void LoadGame()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogWarning("No save file found at: " + SaveFilePath);
            return;
        }

        string json = File.ReadAllText(SaveFilePath);
        SaveData data = null;
        try
        {
            data = JsonUtility.FromJson<SaveData>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to parse save file: " + e.Message);
            return;
        }

        var playerInv = Object.FindObjectOfType<PlayerInventory>();
        if (playerInv == null)
        {
            Debug.LogError("Load failed: no PlayerInventory found in scene.");
            return;
        }

        playerInv.transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
        playerInv.SetNumberOfPapers(data.numberOfPapers);

        // Restore paper active states by matching positions
        var allPapers = Resources.FindObjectsOfTypeAll<Paper>();
        var activeScene = SceneManager.GetActiveScene();
        var unmatched = new List<PaperEntry>(data.papers);

        const float matchEpsilon = 0.05f;
        foreach (var p in allPapers)
        {
            if (p == null || p.gameObject == null) continue;
            if (p.gameObject.scene != activeScene) continue;

            Vector3 pos = p.transform.position;
            int matchIndex = -1;
            for (int i = 0; i < unmatched.Count; i++)
            {
                var pe = unmatched[i];
                if (Mathf.Abs(pe.x - pos.x) < matchEpsilon && Mathf.Abs(pe.y - pos.y) < matchEpsilon && Mathf.Abs(pe.z - pos.z) < matchEpsilon)
                {
                    matchIndex = i;
                    break;
                }
            }

            if (matchIndex >= 0)
            {
                var pe = unmatched[matchIndex];
                p.gameObject.SetActive(pe.isActive);
                unmatched.RemoveAt(matchIndex);
            }
            else
            {
                // if no saved entry, leave as-is or optionally deactivate
            }
        }

        if (unmatched.Count > 0)
        {
            Debug.LogWarning(unmatched.Count + " saved papers could not be matched to scene objects.");
        }

        // Update UI if any listeners exist
        if (playerInv != null)
        {
            // Trigger event to update UI
            // PlayerInventory.SetNumberOfPapers already invokes OnPaperCollected inside it, so UI updates.
        }

        Debug.Log("Load complete from: " + SaveFilePath);
    }
}
