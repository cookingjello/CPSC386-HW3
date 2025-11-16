// https://www.youtube.com/watch?v=EfUCEwKmcjc


using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{

    public int NumberOfPapers { get; private set; }

    public UnityEvent<PlayerInventory> OnPaperCollected;

public void PaperCollected()
    {
        NumberOfPapers++;
        if (OnPaperCollected != null)
        {
            try
            {
                OnPaperCollected.Invoke(this);
            }
            catch (ArgumentException e)
            {
                Debug.LogError("OnPaperCollected listener argument mismatch: " + e.Message);
            }
        }
    }

    // Restore the paper count (used by save/load). // AI-ADDED
    public void SetNumberOfPapers(int count) // AI-ADDED
    {
        NumberOfPapers = Mathf.Max(0, count); // AI-ADDED
        if (OnPaperCollected != null) // AI-ADDED
        {
            try { OnPaperCollected.Invoke(this); } catch { }
        }
    }

}
