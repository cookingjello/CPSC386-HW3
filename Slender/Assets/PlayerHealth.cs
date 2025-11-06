using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public HealthBar healthBar;
    public GameObject gameOverPanel; // assign your panel in the inspector

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
            GameOver();
    }

    void GameOver()
    {
        Time.timeScale = 0f; // pause game
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    // optional reset for debugging / restart button
    public void Restart()
    {
        Time.timeScale = 1f;
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // Expose current health so SaveManager can persist it
    // Expose current health so SaveManager can persist it // AI-ADDED
    public int GetCurrentHealth() // AI-ADDED
    {
        return currentHealth; // AI-ADDED
    } // AI-ADDED

    public void SetCurrentHealth(int health) // AI-ADDED
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth); // AI-ADDED
        if (healthBar != null) // AI-ADDED
            healthBar.SetHealth(currentHealth); // AI-ADDED

        if (gameOverPanel != null) // AI-ADDED
            gameOverPanel.SetActive(currentHealth <= 0); // AI-ADDED
    } // AI-ADDED

    // Optionally allow changing max health (keeps current health valid) // AI-ADDED
    public void SetMaxHealth(int max) // AI-ADDED
    {
        maxHealth = max; // AI-ADDED
        if (healthBar != null) // AI-ADDED
            healthBar.SetMaxHealth(maxHealth); // AI-ADDED
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // AI-ADDED
        if (healthBar != null) // AI-ADDED
            healthBar.SetHealth(currentHealth); // AI-ADDED
    } // AI-ADDED
}
