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
    private class ChestEntry
    {
        public string chestID;
        public bool isOpened;
    }

    [System.Serializable]
    private class PowerupEntry
    {
        public float x;
        public float y;
        public float z;
        public bool isActive;
        public string type; // e.g., "Health" or "Speed"
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
        public float playerSpeed; // AI-ADDED: persist player movement speed (for speed powerup)
        public List<PaperEntry> papers = new List<PaperEntry>();
        public List<ChestEntry> chests = new List<ChestEntry>(); // AI-ADDED
        public List<PowerupEntry> powerups = new List<PowerupEntry>(); // AI-ADDED
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

        // Save player speed (Movement component) so speed powerups persist
        var playerMovement = Object.FindFirstObjectByType<Movement>();
        if (playerMovement != null)
        {
            data.playerSpeed = playerMovement.speed;
        }

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

        // Save all Chest open states // AI-ADDED
        var allChests = Resources.FindObjectsOfTypeAll<Chest>(); // AI-ADDED
        foreach (var chest in allChests) // AI-ADDED
        {
            if (chest == null || chest.gameObject == null) continue; // AI-ADDED
            if (chest.gameObject.scene != activeScene) continue; // AI-ADDED

            ChestEntry ce = new ChestEntry(); // AI-ADDED
            ce.chestID = chest.ChestID; // AI-ADDED
            ce.isOpened = chest.IsOpened; // AI-ADDED
            data.chests.Add(ce); // AI-ADDED
        } // AI-ADDED

        // Save powerup pickup active states (health/speed)
        var allHealthPowerups = Resources.FindObjectsOfTypeAll<HealthPowerup>();
        foreach (var pu in allHealthPowerups)
        {
            if (pu == null || pu.gameObject == null) continue;
            if (pu.gameObject.scene != activeScene) continue;

            PowerupEntry pue = new PowerupEntry();
            var pos = pu.transform.position;
            pue.x = pos.x; pue.y = pos.y; pue.z = pos.z;
            pue.isActive = pu.gameObject.activeSelf;
            pue.type = "Health";
            data.powerups.Add(pue);
        }

        var allSpeedPowerups = Resources.FindObjectsOfTypeAll<SpeedPowerup>();
        foreach (var pu in allSpeedPowerups)
        {
            if (pu == null || pu.gameObject == null) continue;
            if (pu.gameObject.scene != activeScene) continue;

            PowerupEntry pue = new PowerupEntry();
            var pos = pu.transform.position;
            pue.x = pos.x; pue.y = pos.y; pue.z = pos.z;
            pue.isActive = pu.gameObject.activeSelf;
            pue.type = "Speed";
            data.powerups.Add(pue);
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

        // Restore player speed if Movement exists
        var playerMovement = Object.FindObjectOfType<Movement>();
        if (playerMovement != null && data.playerSpeed > 0f)
        {
            playerMovement.speed = data.playerSpeed;
        }

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

            // If we are continuing from a save and the default spawner skipped spawning, try to instantiate missing papers
            var spawner = Object.FindObjectOfType<PageSpawner>();
            if (spawner != null && spawner.pagePrefab != null)
            {
                Debug.Log("SaveManager: instantiating " + unmatched.Count + " saved papers using PageSpawner.pagePrefab.");
                foreach (var pe in new List<PaperEntry>(unmatched))
                {
                    try
                    {
                        var go = Object.Instantiate(spawner.pagePrefab, new Vector3(pe.x, pe.y, pe.z), Quaternion.identity);
                        go.SetActive(pe.isActive);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Failed to instantiate saved paper prefab: " + e.Message);
                    }
                }
                unmatched.Clear();
            }
        }

        // Restore chest open states by ChestID // AI-ADDED
        var allChests = Resources.FindObjectsOfTypeAll<Chest>(); // AI-ADDED
        foreach (var chest in allChests) // AI-ADDED
        {
            if (chest == null || chest.gameObject == null) continue; // AI-ADDED
            if (chest.gameObject.scene != activeScene) continue; // AI-ADDED

            var savedChest = data.chests.Find(c => c.chestID == chest.ChestID); // AI-ADDED
            if (savedChest != null && savedChest.isOpened) // AI-ADDED
            {
                chest.SetOpened(true); // AI-ADDED
                Debug.Log("Restored chest " + chest.ChestID + " to opened state."); // AI-ADDED
            } // AI-ADDED
        } // AI-ADDED

        // Restore powerup active states by position and type
        var allHealthPUs = Resources.FindObjectsOfTypeAll<HealthPowerup>();
        var allSpeedPUs = Resources.FindObjectsOfTypeAll<SpeedPowerup>();

        var unmatchedPowerups = new List<PowerupEntry>(data.powerups);

        // helper local function to try match a powerup component to saved entries
        System.Action<Transform, string> matchPowerup = (t, type) =>
        {
            var pos = t.position;
            int found = -1;
            for (int i = 0; i < unmatchedPowerups.Count; i++)
            {
                var pe = unmatchedPowerups[i];
                if (pe.type != type) continue;
                if (Mathf.Abs(pe.x - pos.x) < matchEpsilon && Mathf.Abs(pe.y - pos.y) < matchEpsilon && Mathf.Abs(pe.z - pos.z) < matchEpsilon)
                {
                    found = i; break;
                }
            }

            if (found >= 0)
            {
                var pe = unmatchedPowerups[found];
                t.gameObject.SetActive(pe.isActive);
                unmatchedPowerups.RemoveAt(found);
            }
        };

        foreach (var pu in allHealthPUs)
        {
            if (pu == null || pu.gameObject == null) continue;
            if (pu.gameObject.scene != activeScene) continue;
            matchPowerup(pu.transform, "Health");
        }

        foreach (var pu in allSpeedPUs)
        {
            if (pu == null || pu.gameObject == null) continue;
            if (pu.gameObject.scene != activeScene) continue;
            matchPowerup(pu.transform, "Speed");
        }

        if (unmatchedPowerups.Count > 0)
        {
            Debug.LogWarning(unmatchedPowerups.Count + " saved powerups could not be matched to scene objects.");
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
