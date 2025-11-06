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
