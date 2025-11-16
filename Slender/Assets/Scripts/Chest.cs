using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{

    public bool IsOpened { get; private set; }
    public string ChestID { get; private set; }

    public GameObject itemPrefab;
    public Sprite openedSprite;


 private AudioManager audioManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
        audioManager = AudioManager.Instance;

    }


    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    public void OpenChest()
    {
        SetOpened(true);
        if (audioManager != null && audioManager.chestOpen != null) audioManager.PlayChest(audioManager.chestOpen);
        if (itemPrefab)
        {
            
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
           

        }
    }

    public void SetOpened(bool opened)
    {
        IsOpened = opened;
        if (IsOpened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            
            Debug.Log("Chest opened: " + ChestID);
        }
    }
}

