// https://www.youtube.com/watch?v=EfUCEwKmcjc


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
            catch (System.ArgumentException e)
            {
                Debug.LogError("OnPaperCollected listener argument mismatch: " + e.Message);
            }
        }
    }

    // Called by save/load system to restore the number of papers collected.
    public void SetNumberOfPapers(int count)
    {
        NumberOfPapers = count;
        if (OnPaperCollected != null)
        {
            // Update any UI listeners with the restored value.
            try { OnPaperCollected.Invoke(this); } catch { }
        }
    }

}
