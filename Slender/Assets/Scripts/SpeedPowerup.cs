/*
Some aspects and lines of this script were modified or added using VS Code Copilot AI. 
Lines specifically added or modified by AI are marked with "AI-ADDED" comments.
*/
using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    public float increase = 2f;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameObject player = collision.gameObject;
            Movement playerScript = player.GetComponent<Movement>();

            if (playerScript)
            {
                playerScript.speed += increase; //AI ADDED
                if (AudioManager.Instance != null && AudioManager.Instance.speedPowerUp != null) AudioManager.Instance.PlayPowerUp(AudioManager.Instance.speedPowerUp); //AI ADDED
                // Deactivate instead of destroying so save/load can persist powerup state
                gameObject.SetActive(false); //AI ADDED
            }
        }
    }
}
