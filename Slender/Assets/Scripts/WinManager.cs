/*
    AI assisted with formatting this script, as well as adding debug logs to properly identify any problems the code was having
*/


using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private int totalPages = 8;

    private void Start()
    {
        if (playerInventory != null)
        {
            playerInventory.OnPaperCollected.AddListener(OnPaperCollected);
        }

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    private void OnPaperCollected(PlayerInventory inventory)
    {
        if (inventory.NumberOfPapers >= totalPages)
        {
            WinGame();
        }
    }

    [System.Obsolete]
    private void WinGame()
    {
        Time.timeScale = 0f;
        if (winPanel != null)
            winPanel.SetActive(true);

        Debug.Log("All pages collected! YOU WIN!"); // AI-ADDED

        // If the win panel contains a WinScreen component, pass the current timer value so it can show/update the best time // AI-ADDED
        if (winPanel != null) // AI-ADDED
        {
            // try multiple ways to locate the WinScreen: on the panel itself, as a child, or on this object
            WinScreen winScreen = winPanel.GetComponent<WinScreen>(); // AI-ADDED
            if (winScreen == null) winScreen = winPanel.GetComponentInChildren<WinScreen>(); // AI-ADDED
            if (winScreen == null) winScreen = GetComponent<WinScreen>(); // AI-ADDED

            if (winScreen != null) // AI-ADDED
            {
                var timer = Object.FindObjectOfType<Timer>(); // AI-ADDED
                float elapsed = 0f; // AI-ADDED
                if (timer != null) // AI-ADDED
                    elapsed = timer.GetElapsedTime(); // AI-ADDED

                winScreen.RecordWin(elapsed); // AI-ADDED
            }
            else // AI-ADDED
            {
                Debug.LogWarning("WinManager: WinScreen component not found on winPanel or as a child; attach WinScreen to the win panel or the WinManager object."); // AI-ADDED
            }
        }
    }

  public void GoToMenu()
{
    Debug.Log("GoToMenu() called"); // AI-ADDED
    Time.timeScale = 1f;
    SceneManager.LoadScene("Menu");
}


}
