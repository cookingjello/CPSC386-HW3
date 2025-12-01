/*
    This script was written using a YouTube video as guidance:
    https://www.youtube.com/watch?v=EfUCEwKmcjc
    YouTube channel: Ketra Games

    Some aspects and lines of this script were modified or added using VS Code Copilot AI. 
    Lines specifically added or modified by AI are marked with "AI-ADDED" comments.
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
