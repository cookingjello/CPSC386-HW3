using UnityEngine;

public class HealthPowerup : MonoBehaviour
{
    public int increase = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject player = collision.gameObject;
            PlayerHealth playerScript = player.GetComponent<PlayerHealth>();

            if (playerScript)
            {
                int current = playerScript.GetCurrentHealth();

                // AI-ADDED
                if (current >= playerScript.maxHealth) // AI-ADDED
                    return; // AI-ADDED

                int newHealth = current + increase; // AI-ADDED
                if (newHealth > playerScript.maxHealth) newHealth = playerScript.maxHealth; // AI-ADDED

                playerScript.SetCurrentHealth(newHealth);
                Destroy(gameObject); 
            }
        }
    }
}