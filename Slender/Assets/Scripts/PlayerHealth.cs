/*
    This script was made using mostly AI assistance..
*/


using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public HealthBar healthBar;
    public GameObject gameOverPanel;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TakeDamage(int damage) //AI-ADDED
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);

        var audioMgr = AudioManager.Instance; //AI-ADDED
        if (audioMgr != null && audioMgr.health != null) audioMgr.PlayHealth(audioMgr.health); //AI-ADDED

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

    // Heal the player by a (float) amount. Accepts fractional values and rounds to nearest int. // AI-ADDED
    public void Heal(float amount) // AI-ADDED
    {
        int inc = Mathf.RoundToInt(amount); // AI-ADDED
        currentHealth = Mathf.Clamp(currentHealth + inc, 0, maxHealth); // AI-ADDED
        if (healthBar != null) healthBar.SetHealth(currentHealth); // AI-ADDED
        Debug.Log("HealthPowerUp: Player healed. Current health: " + currentHealth); //AI ADDED
    } // AI-ADDED

    
}
