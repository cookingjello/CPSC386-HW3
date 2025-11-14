/*
This script was written with guidance from the following tutorial:
https://www.youtube.com/watch?v=N8whM1GjH4w
Youtube Channel: Rehope Games

Some aspects and lines of this script were modified or added using VS Code Copilot AI. 
Lines specifically added or modified by AI are marked with "AI-ADDED" comments.
*/

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } // AI-ADDED

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] private AudioMixer myMixer;

    public AudioClip background;
    public AudioClip paper;
    public AudioClip health;
    public AudioClip speedPowerUp; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // AI-ADDED
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {
        if (Instance == this) // AI-ADDED
        {
            musicSource.clip = background;
            musicSource.loop = true; // AI-ADDED
            musicSource.Play();
        }
    }

    public void PlayPaper(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayHealth(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayPowerUp(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip); //AI ADDED
    }
}