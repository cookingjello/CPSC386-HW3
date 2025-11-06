using System.Collections;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    IEnumerator Start()
    {
        if (SaveManager.LoadAfterSceneLoad)
        {
            // wait one frame to allow all Awake/Start on scene objects to run,
            // so objects like PlayerInventory or movement scripts don't overwrite the loaded position.
            yield return null;

            SaveManager.LoadAfterSceneLoad = false;
            Debug.Log("Applying saved game...");
            SaveManager.LoadGame();
        }
    }
}
