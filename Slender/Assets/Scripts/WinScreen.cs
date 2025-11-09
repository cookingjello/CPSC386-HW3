using UnityEngine;
using TMPro;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentTimeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private GameObject newRecordBadge; // optional visual for new record

    // Optional: scope bests per-level using the scene name or an id
    private string BestTimeKey(string levelId = null)
    {
        if (string.IsNullOrEmpty(levelId))
            levelId = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        return $"best_time_{levelId}";
    }

    // Call this when player wins
    public void RecordWin(float currentElapsedSeconds, string levelId = null)
    {
        string key = BestTimeKey(levelId);
        float best = GetBestTime(key);

        bool isNewRecord = false;
        // Lower time is better; treat 0 or negative as "no record"
        if (best <= 0f || currentElapsedSeconds < best)
        {
            SaveBestTime(key, currentElapsedSeconds);
            best = currentElapsedSeconds;
            isNewRecord = true;
        }

        // Update UI
        if (currentTimeText != null) currentTimeText.text = FormatTime(currentElapsedSeconds);
        if (bestTimeText != null) bestTimeText.text = FormatTime(best);
        if (newRecordBadge != null) newRecordBadge.SetActive(isNewRecord);
    }

    private float GetBestTime(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetFloat(key);
        return 0f; // 0 = no record saved
    }

    private void SaveBestTime(string key, float seconds)
    {
        PlayerPrefs.SetFloat(key, seconds);
        PlayerPrefs.Save();
    }

    private string FormatTime(float timeSeconds)
    {
        int minutes = Mathf.FloorToInt(timeSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeSeconds % 60f);
        int ms = Mathf.FloorToInt((timeSeconds - Mathf.Floor(timeSeconds)) * 1000f); // optional milliseconds
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, ms);
        // If you prefer mm:ss only: return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}