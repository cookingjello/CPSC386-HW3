using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.IO;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    public GameObject optionsPanel;
       
    public void ContinueGame()
{
    if (File.Exists(SaveManager.SaveFilePath))
    {
        Debug.Log("Will load saved game after scene loads...");
        SaveManager.LoadAfterSceneLoad = true;
    }
    else
    {
        Debug.LogWarning("No save file found! Starting a new game instead.");
        SaveManager.LoadAfterSceneLoad = false;
    }

    SceneManager.LoadScene("Game");
}




    public void GameMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void Start()
    {
        if (musicSlider != null && myMixer != null)
        {
            if (PlayerPrefs.HasKey("musicVolume"))
                LoadMusicVolume();
            else
                SetMusicVolume();
        }
        else
        {
            Debug.LogWarning("MenuManager: musicSlider or myMixer not assigned in inspector.");
        }

        if (SFXSlider != null && myMixer != null)
        {
            if (PlayerPrefs.HasKey("SFXVolume"))
                LoadSFXVolume();
            else
                SetSFXVolume();
        }
        else
        {
            Debug.LogWarning("MenuManager: SFXSlider or myMixer not assigned in inspector.");
        }
    }

    public void SetMusicVolume()
    {
        if (musicSlider == null || myMixer == null) { Debug.LogWarning("MenuManager: cannot SetMusicVolume - missing references."); return; } // AI-ADDED
        float musicVolume = musicSlider.value; // AI-ADDED
        myMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20); // AI-ADDED
        PlayerPrefs.SetFloat("musicVolume", musicVolume); // AI-ADDED
    }

    public void SetSFXVolume()
    {
        if (SFXSlider == null || myMixer == null) { Debug.LogWarning("MenuManager: cannot SetSFXVolume - missing references."); return; } // AI-ADDED
        float SFXVolume = SFXSlider.value; // AI-ADDED
        myMixer.SetFloat("SFX", Mathf.Log10(SFXVolume) * 20); // AI-ADDED
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume); // AI-ADDED
    }

    private void LoadMusicVolume()
    {
        if (musicSlider == null) { Debug.LogWarning("MenuManager: cannot LoadMusicVolume - musicSlider is null."); return; } // AI-ADDED
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume"); // AI-ADDED
        SetMusicVolume(); // AI-ADDED
    }
     
    private void LoadSFXVolume()
    {
        if (SFXSlider == null) { Debug.LogWarning("MenuManager: cannot LoadSFXVolume - SFXSlider is null."); return; } // AI-ADDED
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume"); // AI-ADDED
        SetSFXVolume(); // AI-ADDED
    }       

    public void StartGameScene()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetFloat("player_moveSpeed", 1.2f);
        PlayerPrefs.SetInt("player_score", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
    }

    public void GameQuit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void ShowOptions() => optionsPanel.SetActive(true);
    public void HideOptions() => optionsPanel.SetActive(false);
}
