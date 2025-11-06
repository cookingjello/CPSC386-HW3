/*

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

    private void WinGame()
    {
        Time.timeScale = 0f;
        if (winPanel != null)
            winPanel.SetActive(true);

        Debug.Log("All pages collected! YOU WIN!"); // AI-ADDED
    }

  public void GoToMenu()
{
    Debug.Log("GoToMenu() called"); // AI-ADDED
    Time.timeScale = 1f;
    SceneManager.LoadScene("Menu");
}


}
