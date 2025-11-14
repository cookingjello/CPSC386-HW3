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
                Destroy(gameObject); //AI ADDED
            }
        }
    }
}
