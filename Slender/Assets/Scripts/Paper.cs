// https://www.youtube.com/watch?v=EfUCEwKmcjc

using UnityEngine;

public class Paper : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other) 
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            playerInventory.PaperCollected();
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}


