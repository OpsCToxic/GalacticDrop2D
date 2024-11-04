using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public GameObject player; // Reference to the Player GameObject
    public Health playerHealth; // Reference to the Health component
    public Image healthbar; // Reference to the health bar UI Image

    void Start()
    {
        if (player != null)
        {
            // Get the Health component from the Player GameObject
            playerHealth = player.GetComponent<Health>();

            // Initialize the health bar fill based on the player's current health
            if (playerHealth != null)
            {
                // Subscribe to the health changed event
                playerHealth.OnHealthChanged.AddListener(UpdateHealthBar);
                // Initialize the health bar fill based on max health
                UpdateHealthBar(playerHealth.MaxHp);
            }
        }
    }

    void Update()
    {
        // Check if the player's health is 0 or below
        if (playerHealth != null && playerHealth.Hp <= 0)
        {
            OnPlayerDeath();
        }
    }

    public void TakeDamage(float damage)
    {
        // Check if playerHealth is available
        if (playerHealth != null)
        {
            playerHealth.Damage((int)damage); // Apply damage to the player's health
            UpdateHealthBar(playerHealth._curHp);
        }
    }

    public void Heal(float healingAmount)
    {
        // Check if playerHealth is available
        if (playerHealth != null)
        {
            playerHealth.Heal((int)healingAmount); // Heal the player's health
            UpdateHealthBar(playerHealth._curHp);
        }
    }

    private void UpdateHealthBar(int currentHealth)
    {
        // Update the health bar fill amount based on the player's current health
        float fillAmount = (float)currentHealth / playerHealth.MaxHp;
        healthbar.fillAmount = fillAmount;
    }

    private void OnPlayerDeath()
    {
        // Reload the current scene to reset the game when the player dies
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
