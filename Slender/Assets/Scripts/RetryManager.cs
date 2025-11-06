using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryManager : MonoBehaviour
{
    // Call this from a button OnClick or another script
    public void TryAgain()
    {
        Time.timeScale = 1f; // unfreeze the game if paused
        SceneManager.LoadScene("Game");
    }
}
