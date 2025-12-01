/*
Some aspects and lines of this script were modified or added using VS Code Copilot AI. 
Lines specifically added or modified by AI are marked with "AI-ADDED" comments.
*/

using UnityEngine;

public class HealthPowerup : MonoBehaviour
{
    public int increase = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject player = collision.gameObject;
            PlayerHealth playerScript = player.GetComponent<PlayerHealth>();

            if (playerScript)
            {

                
                int current = playerScript.GetCurrentHealth();
if (AudioManager.Instance != null && AudioManager.Instance.healthPowerUp != null) AudioManager.Instance.PlayPowerUp(AudioManager.Instance.healthPowerUp); //AI ADDED
                // AI-ADDED
                if (current >= playerScript.maxHealth) // AI-ADDED
                    return; // AI-ADDED

                int newHealth = current + increase; // AI-ADDED
                if (newHealth > playerScript.maxHealth) newHealth = playerScript.maxHealth; // AI-ADDED

                playerScript.SetCurrentHealth(newHealth);
                // Instead of destroying the pickup, deactivate it so it can be saved and restored by SaveManager
                gameObject.SetActive(false);
            }
        }
    }
}