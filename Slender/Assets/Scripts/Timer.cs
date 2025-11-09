using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    // Expose elapsed time so SaveManager can persist and restore it // AI-ADDED
    public float GetElapsedTime() // AI-ADDED
    {
        return elapsedTime; // AI-ADDED
    } // AI-ADDED

    public void SetElapsedTime(float t) // AI-ADDED
    {
        elapsedTime = Mathf.Max(0f, t); // AI-ADDED
        // Immediately refresh visible text so loading shows current time // AI-ADDED
        int minutes = Mathf.FloorToInt(elapsedTime / 60f); // AI-ADDED
        int seconds = Mathf.FloorToInt(elapsedTime % 60f); // AI-ADDED
        if (timerText != null) timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // AI-ADDED
    } // AI-ADDED
    // Reference to GameIntro so we can wait until the intro panel is gone before starting the timer // AI-ADDED
    private GameIntro gameIntro; // AI-ADDED
    // Whether the timer is currently running (only true after intro ends) // AI-ADDED
    private bool timerRunning = false; // AI-ADDED

    void Start() // AI-ADDED
    {
        // Always try to find GameIntro. If found, use the coroutine to wait for the intro lifecycle; // AI-ADDED
        // this avoids racing with GameIntro.Start which may activate the panel after this Start runs. // AI-ADDED
        gameIntro = FindObjectOfType<GameIntro>(); // AI-ADDED
        if (gameIntro == null) // AI-ADDED
        {
            // No intro present, start timer immediately // AI-ADDED
            timerRunning = true; // AI-ADDED
        }
        else // AI-ADDED
        {
            // Start coroutine that will wait up to introDuration for an active intro panel to appear and then finish // AI-ADDED
            StartCoroutine(WaitForIntroThenStartTimer(gameIntro.introDuration)); // AI-ADDED
        }
    }

    private System.Collections.IEnumerator WaitForIntroThenStartTimer(float maxWait) // AI-ADDED
    {
        // Wait one frame so other scripts (GameIntro.Start) have a chance to initialize and activate the panel if they need to // AI-ADDED
        yield return null; // AI-ADDED

        float waited = 0f; // AI-ADDED

        // Wait up to maxWait for either: the intro panel lifecycle (active -> inactive) to complete, or for maxWait to elapse. // AI-ADDED
        while (waited < maxWait)
        {
            // If intro panel becomes active, wait until it is deactivated or maxWait elapses
            if (gameIntro != null && gameIntro.introPanel != null && gameIntro.introPanel.activeSelf) // AI-ADDED
            {
                // Wait while panel is active
                while (waited < maxWait && gameIntro != null && gameIntro.introPanel != null && gameIntro.introPanel.activeSelf) // AI-ADDED
                {
                    waited += Time.deltaTime; // AI-ADDED
                    yield return null; // AI-ADDED
                }

                // Panel was hidden or maxWait elapsed; break out and start timer
                break; // AI-ADDED
            }

            // Panel not yet active; keep waiting in case GameIntro activates it shortly
            waited += Time.deltaTime; // AI-ADDED
            yield return null; // AI-ADDED
        }

        // Either the panel was hidden (skip) or the max duration elapsed — start the timer now // AI-ADDED
        timerRunning = true; // AI-ADDED
    }


    // Update is called once per frame
    void Update()
    {
        
        if (!timerRunning)
            return;

        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); 
    }
}
