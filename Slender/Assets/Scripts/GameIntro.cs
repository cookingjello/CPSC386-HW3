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

    void Start()
    {
        playerMovement = FindObjectOfType<Movement>(); //AI-ADDED
        if (playerMovement != null)
            playerMovement.enabled = false; 

        if (introPanel != null)
            introPanel.SetActive(true);

        StartCoroutine(HideIntroPanelAfterDelay());
    }

    IEnumerator HideIntroPanelAfterDelay() //AI-ADDED
    {
        yield return new WaitForSeconds(introDuration);

        if (introPanel != null)
            introPanel.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true; 
    }
}
