// https://www.youtube.com/watch?v=EfUCEwKmcjc

using UnityEngine;

public class Paper : MonoBehaviour
{
    private AudioManager audioManager; //AI ADDED

    private void Awake() 
    {
        // Prefer the AudioManager singleton if available
        audioManager = AudioManager.Instance; //AI ADDED
        if (audioManager == null)
        {
            // Fallback: try to find an object tagged "Audio" in the scene
            var audioObj = GameObject.FindWithTag("Audio"); //AI ADDED
            if (audioObj != null) audioManager = audioObj.GetComponent<AudioManager>(); //AI ADDED
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            playerInventory.PaperCollected(); 
            // Play the paper pickup sound if available
            if (audioManager != null && audioManager.paper != null) audioManager.PlayPaper(audioManager.paper); //AI ADDED
            gameObject.SetActive(false); 
            //Destroy(gameObject);
        }
    }
}


