/*
This script was written with guidance from the following tutorials:
Audio Management:
https://www.youtube.com/watch?v=G-JUp8AMEx0
Youtube Channel: Rehope Games

Some aspects and lines of this script were modified or added using VS Code Copilot AI.
Lines specifically added or modified by AI are marked with "AI-ADDED" comments.
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    public GameObject optionsPanel;
       
    public void GameMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume();
        }

          if (PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolume();
        }
    }
    public void SetMusicVolume()
    {
        float musicVolume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }

    public void SetSFXVolume()
    {
        float SFXVolume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(SFXVolume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
    }

    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
    }
     
    private void LoadSFXVolume()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetSFXVolume();
    }       

    public void StartGameScene()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetFloat("player_moveSpeed", 1.2f); // AI-ADDED
        PlayerPrefs.Save(); // AI-ADDED
        PlayerPrefs.SetInt("player_score", 0); // AI-ADDED
        PlayerPrefs.Save(); // AI-ADDED
        SceneManager.LoadScene("Game");
    }

    public void GameQuit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

        #endif
        Application.Quit();
    }

    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
    }
    
    public void HideOptions()
    {
        optionsPanel.SetActive(false);
    }
}

