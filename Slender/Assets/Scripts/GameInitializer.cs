/*
    This script was written using a YouTube video as guidance:
    https://youtu.be/XOjd_qU2Ido
    YouTube channel: Brackeys
*/


using System.Collections;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    
    IEnumerator Start()
    {
        if (SaveManager.LoadAfterSceneLoad)
        {

            yield return null;

            SaveManager.LoadAfterSceneLoad = false;
            Debug.Log("Applying saved game...");
            SaveManager.LoadGame();
        }
    }
}
