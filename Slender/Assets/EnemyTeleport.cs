using UnityEngine;

public class EnemyTeleport : MonoBehaviour
{
    public Transform player;                   // Player transform (auto-found if not set)
    public PlayerInventory playerInventory;    // Player inventory (auto-found if not set)

    public float minRadius = 3f;
    public float maxRadius = 8f;

    public float baseMinTime = 5f;
    public float baseMaxTime = 10f;
    public float aggressionFactor = 0.8f;      // Lower = more aggressive faster

    private float teleportTimer;

    void Start()
    {
        // Auto-find player if not assigned
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        // Auto-find PlayerInventory if not assigned
        if (playerInventory == null && player != null)
            playerInventory = player.GetComponent<PlayerInventory>();

        ResetTeleportTimer();
    }

    void Update()
    {
        teleportTimer -= Time.deltaTime;

        if (teleportTimer <= 0f)
        {
            TeleportAroundPlayer();
            ResetTeleportTimer();
        }
    }

    void TeleportAroundPlayer()
    {
        if (player == null) return;

        Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
        Vector2 newPosition = (Vector2)player.position + randomOffset;
        transform.position = newPosition;
    }

    void ResetTeleportTimer()
    {
        float minTime = baseMinTime;
        float maxTime = baseMaxTime;

        if (playerInventory != null)
        {
            int papers = playerInventory.NumberOfPapers;
            float aggressionMultiplier = Mathf.Pow(aggressionFactor, papers);

            minTime *= aggressionMultiplier;
            maxTime *= aggressionMultiplier;
        }

        teleportTimer = Random.Range(minTime, maxTime);
    }
}
