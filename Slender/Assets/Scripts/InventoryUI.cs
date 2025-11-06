/*
    
*/


using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI paperText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        paperText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void UpdatePaperText(PlayerInventory playerInventory)
    {
        paperText.text = playerInventory.NumberOfPapers.ToString() + " / 8" ;
    }
}
