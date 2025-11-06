using System.Collections;
using UnityEngine;

public class GameIntro : MonoBehaviour
{
    public GameObject introPanel;
    public float introDuration = 5f; // how long to show the panel
    private Movement playerMovement; // or whatever your movement script is called

    void Start()
    {
        // Find the player and its movement component
        playerMovement = FindObjectOfType<Movement>(); 
        if (playerMovement != null)
            playerMovement.enabled = false; // disable player control initially

        // Show the intro panel
        if (introPanel != null)
            introPanel.SetActive(true);

        // Start coroutine to hide the panel after a few seconds
        StartCoroutine(HideIntroPanelAfterDelay());
    }

    IEnumerator HideIntroPanelAfterDelay()
    {
        yield return new WaitForSeconds(introDuration);

        if (introPanel != null)
            introPanel.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true; // re-enable player control
    }
}
