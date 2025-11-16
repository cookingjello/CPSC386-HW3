/*
    This script was made with the assistance of a YouTube video: 
    https://www.youtube.com/watch?v=XOjd_qU2Ido
    Channel: Brackeys

    AI also assisted in making this script in areas where the tutorial was not sufficient.

*/


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
        public int playerCurrentHealth; // AI-ADDED
        public int playerMaxHealth; // AI-ADDED
        public float playerElapsedTime; // AI-ADDED
        public int numberOfPapers;
        public List<PaperEntry> papers = new List<PaperEntry>();
    }

    
    public static void SaveGame() // AI-ADDED
    {
        var playerInv = Object.FindFirstObjectByType<PlayerInventory>();
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

        // Save player health if a PlayerHealth component exists // AI-ADDED
        var playerHealth = Object.FindFirstObjectByType<PlayerHealth>(); // AI-ADDED
        if (playerHealth != null) // AI-ADDED
        {
            data.playerCurrentHealth = playerHealth.GetCurrentHealth(); // AI-ADDED
            data.playerMaxHealth = playerHealth.maxHealth; // AI-ADDED
        } // AI-ADDED

        // Save timer if present // AI-ADDED
        var timer = Object.FindFirstObjectByType<Timer>(); // AI-ADDED
        if (timer != null) // AI-ADDED
        {
            data.playerElapsedTime = timer.GetElapsedTime(); // AI-ADDED
        } // AI-ADDED

        // Find all Paper components in the current scene (including inactive)
        var allPapers = Resources.FindObjectsOfTypeAll<Paper>();
        var activeScene = SceneManager.GetActiveScene();
        foreach (var p in allPapers) // AI-ADDED
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

    [System.Obsolete]
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

        // Restore player health if possible // AI-ADDED
        var playerHealth = Object.FindObjectOfType<PlayerHealth>(); // AI-ADDED
        if (playerHealth != null) // AI-ADDED
        {
            // If the saved max health differs, apply it first so current clamps correctly // AI-ADDED
            if (data.playerMaxHealth > 0) // AI-ADDED
                playerHealth.SetMaxHealth(data.playerMaxHealth); // AI-ADDED

            playerHealth.SetCurrentHealth(data.playerCurrentHealth); // AI-ADDED
        } // AI-ADDED

        // Restore timer if present // AI-ADDED
        var timer = Object.FindObjectOfType<Timer>(); // AI-ADDED
        if (timer != null) // AI-ADDED
        {
            timer.SetElapsedTime(data.playerElapsedTime); // AI-ADDED
        } // AI-ADDED

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
