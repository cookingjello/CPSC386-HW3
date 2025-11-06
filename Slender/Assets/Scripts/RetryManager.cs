using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryManager : MonoBehaviour
{
    public void TryAgain()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Game");
    }
}
