using UnityEngine;

public class PageSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;      // All possible positions (30)
    public GameObject pagePrefab;        // Your Paper.prefab
    public int pagesToSpawn = 7;         // How many pages should appear

    void Start()
    {
        SpawnPages();
    }

    void SpawnPages()
    {
        // Shuffle the spawn points
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int rand = Random.Range(i, spawnPoints.Length);
            (spawnPoints[i], spawnPoints[rand]) = (spawnPoints[rand], spawnPoints[i]);
        }

        // Spawn the first N points in the shuffled list
        for (int i = 0; i < pagesToSpawn; i++)
        {
            Instantiate(pagePrefab, spawnPoints[i].position, Quaternion.identity);
        }
    }
}
