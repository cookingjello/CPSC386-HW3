/*
This script was written with guidance from the following Youtube tutorial playlist:
https://www.youtube.com/playlist?list=PLSR2vNOypvs5jmv1YIP_IVtlPnU5yEunL
Youtube Channel: Night Run Studios
Playlist: Let's Make An Arcade Game Like Space Invaders!

Some aspects and lines of this script were modified or added using VS Code Copilot AI.
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    private bool isPaused;

    public GameObject pausePanel;
    public GameObject introPanel; // UNITY 4

    void Update()
    {
        if (introPanel != null && introPanel.activeSelf) // UnITY 4
        {
            return; // Don't allow pausing if intro panel is active
        }
        
        if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        pausePanel.SetActive(false);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void SaveGame()
    {
        SaveManager.SaveGame(); //AI ADDED
        Debug.Log("Game saved to: " + SaveManager.SaveFilePath); //AI ADDED
    }
}
