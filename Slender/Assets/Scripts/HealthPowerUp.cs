using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    public float healthInc = 20;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HealthPowerUp: OnTriggerEnter2D called with object: " + collision.gameObject.name); //AI ADDED
        if(collision.tag == "Player")
        {
            GameObject player = collision.gameObject;
            Debug.Log("HealthPowerUp: Player tag matched!"); //AI ADDED
            PlayerHealth playerScript = player.GetComponent<PlayerHealth>();

            if (playerScript)
            {
                Debug.Log("HealthPowerUp: PlayerHealth found, healing player..."); //AI ADDED
                playerScript.Heal(healthInc); //AI ADDED
                Debug.Log("HealthPowerUp: Heal called successfully."); //AI ADDED
                if (AudioManager.Instance != null && AudioManager.Instance.healthPowerUp != null) AudioManager.Instance.PlayPowerUp(AudioManager.Instance.healthPowerUp); //AI ADDED
                Destroy(gameObject); //AI ADDED
            } else {
                Debug.LogWarning("HealthPowerUp: PlayerHealth component not found on " + player.name); //AI ADDED
            }
        } else {
            Debug.Log("HealthPowerUp: Tag mismatch. Collision object tag is: '" + collision.tag + "'"); //AI ADDED
        }
    }
}
