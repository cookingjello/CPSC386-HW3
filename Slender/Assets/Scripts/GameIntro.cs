/*
    The intention of this script is to display an introductory panel at the start of the game, disabling player movement during this time.
    After a set duration, the panel is hidden and player movement is re-enabled.
    This script was written with a small amount of help from AI.
*/


using System.Collections;
using UnityEngine;

public class GameIntro : MonoBehaviour
{
    public GameObject introPanel;
    public float introDuration = 5f; 
    private Movement playerMovement; 
    // Reference to running coroutine so we can stop it if skipped // AI-ADDED
    private Coroutine hideCoroutine; // AI-ADDED

    void Start()
    {
        playerMovement = FindObjectOfType<Movement>(); //AI-ADDED
        if (playerMovement != null)
            playerMovement.enabled = false; 

        if (introPanel != null)
            introPanel.SetActive(true);

        hideCoroutine = StartCoroutine(HideIntroPanelAfterDelay()); // AI-ADDED
    }

    // Intentionally no Update() key-based skipping; SkipIntro() is public for UI Button use only // AI-ADDED

    IEnumerator HideIntroPanelAfterDelay() //AI-ADDED
    {
        yield return new WaitForSeconds(introDuration);

        ApplyEndIntro(); // AI-ADDED
    }

    // Public method so a UI button can call it to skip the intro immediately // AI-ADDED
    public void SkipIntro() // AI-ADDED
    {
        if (hideCoroutine != null) // AI-ADDED
        {
            StopCoroutine(hideCoroutine); // AI-ADDED
            hideCoroutine = null; // AI-ADDED
        }

        ApplyEndIntro(); // AI-ADDED
    }

    // Centralized logic for ending the intro (hide panel + enable movement) // AI-ADDED
    private void ApplyEndIntro() // AI-ADDED
    {
        if (introPanel != null) // AI-ADDED
            introPanel.SetActive(false); // AI-ADDED

        if (playerMovement != null) // AI-ADDED
            playerMovement.enabled = true; // AI-ADDED
    }
}
