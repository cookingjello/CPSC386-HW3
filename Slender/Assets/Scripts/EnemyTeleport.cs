/*
    This script was written with AI using a prompt that could easily be understood by a human.  The aggression factor is inspired by the aggression 
    scaling in Five Nights at Freddy's.  The teleporting location and frequency is inspired by the original slenderman code.
*/
using UnityEngine;

public class EnemyTeleport : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Teleport Settings")]
    public float minRadius = 3f;
    public float maxRadius = 8f;
    public float baseMinTime = 5f;
    public float baseMaxTime = 10f;
    public float aggressionFactor = 0.8f;
    // Layer mask used to treat those colliders as invalid teleport targets (e.g. Walls) // AI-ADDED
    [Tooltip("Layers that should block teleporting (e.g. Walls)")]
    public LayerMask wallLayerMask; // AI-ADDED
    // How large an overlap check to use when testing a candidate teleport position // AI-ADDED
    public float teleportCheckRadius = 0.3f; // AI-ADDED
    // How many random attempts before giving up and accepting the last candidate // AI-ADDED
    public int maxTeleportAttempts = 20; // AI-ADDED

    [Header("Attack Settings")]
    public float damageRadius = 2f;    // how close player must be to take damage
    public int damageAmount = 10;
    public float damageCooldown = 2f;  // seconds between damage ticks

    private float teleportTimer;
    private float damageTimer;

    void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        if (playerInventory == null && player != null)
            playerInventory = player.GetComponent<PlayerInventory>();

        if (playerHealth == null && player != null)
            playerHealth = player.GetComponent<PlayerHealth>();

        ResetTeleportTimer();

        // If the designer hasn't assigned a wall layer mask in the Inspector, try to auto-find a layer named "Walls" // AI-ADDED
        if (wallLayerMask == 0) // AI-ADDED
        {
            int wallsLayer = LayerMask.NameToLayer("Walls"); // AI-ADDED
            if (wallsLayer != -1) // AI-ADDED
            {
                wallLayerMask = 1 << wallsLayer; // AI-ADDED
                Debug.Log("EnemyTeleport: auto-assigned Wall LayerMask to layer 'Walls' (layer index " + wallsLayer + ")"); // AI-ADDED
            }
            else // AI-ADDED
            {
                Debug.LogWarning("EnemyTeleport: no 'Walls' layer found. Create a layer named 'Walls' and assign wall tiles to it to prevent teleporting into walls."); // AI-ADDED
            }
        }
    }

    void Update()
    {
        teleportTimer -= Time.deltaTime;
        damageTimer -= Time.deltaTime;

        if (teleportTimer <= 0f)
        {
            TeleportAroundPlayer();
            ResetTeleportTimer();
        }

        TryDamagePlayer();
    }

    void TeleportAroundPlayer()
    {
        if (player == null) return;

        Vector2 bestCandidate = (Vector2)player.position + Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius); // AI-ADDED
        bool found = false; // AI-ADDED

        for (int i = 0; i < maxTeleportAttempts; i++) // AI-ADDED
        {
            Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius); // AI-ADDED
            Vector2 candidate = (Vector2)player.position + randomOffset; // AI-ADDED

            // Check physics overlap at candidate position against wall layers; if none, accept it // AI-ADDED
            Collider2D hit = Physics2D.OverlapCircle(candidate, teleportCheckRadius, wallLayerMask); // AI-ADDED
            if (hit == null) // AI-ADDED
            {
                bestCandidate = candidate; // AI-ADDED
                found = true; // AI-ADDED
                break; // AI-ADDED
            }
            else // AI-ADDED
            {
                // keep trying; remember last candidate as a fallback // AI-ADDED
                bestCandidate = candidate; // AI-ADDED
            }
        }

        if (!found) // AI-ADDED
        {
            // All attempts overlapped walls; choose the last candidate but nudge it outward until free (simple fallback) // AI-ADDED
            Vector2 dir = ((Vector2)bestCandidate - (Vector2)player.position).normalized; // AI-ADDED
            for (int j = 0; j < 10; j++) // AI-ADDED
            {
                bestCandidate += dir * teleportCheckRadius * 0.5f; // AI-ADDED
                if (Physics2D.OverlapCircle(bestCandidate, teleportCheckRadius, wallLayerMask) == null) // AI-ADDED
                {
                    found = true; // AI-ADDED
                    break; // AI-ADDED
                }
            }
        }

        // Apply chosen position (if still overlapping walls, it will still be placed but we've attempted to avoid that) // AI-ADDED
        transform.position = bestCandidate; // AI-ADDED
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

    void TryDamagePlayer()
    {
        if (player == null || playerHealth == null || damageTimer > 0f)
            return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= damageRadius)
        {
            playerHealth.TakeDamage(damageAmount);
            damageTimer = damageCooldown; // reset cooldown
        }
    }
}
