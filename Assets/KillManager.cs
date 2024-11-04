using System.Diagnostics;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject healthPowerUpPrefab;     // Prefab for the health power-up
    public GameObject upgradePowerUpPrefab;    // Prefab for the upgrade power-up
    public Transform player;                   // Reference to the player
    private HealthManager healthManager;       // Reference to the HealthManager

    private int enemyKillCount = 0;
    private const int killsPerPowerUp = 25;    // Number of kills to spawn a power-up

    void Start()
    {
        // Find the HealthManager component in the scene and assign it
        healthManager = FindObjectOfType<HealthManager>();
    }

    // Called each time an enemy is killed
    public void OnEnemyKilled(Vector3 position)
    {
        enemyKillCount++;

        // If the required kill count is reached, drop power-ups
        if (enemyKillCount >= killsPerPowerUp)
        {
            DropPowerUps(position);
            enemyKillCount = 0; // Reset the counter after dropping power-ups
        }
    }

    // Method to drop power-ups at the enemy's position
    private void DropPowerUps(Vector3 position)
    {
        Instantiate(healthPowerUpPrefab, position, Quaternion.identity);
        Instantiate(upgradePowerUpPrefab, position + new Vector3(1, 0, 0), Quaternion.identity);
    }

    // Method to apply health power-up effect using HealthManager's Heal method
    public void ApplyHealthPowerUp()
    {
        if (healthManager != null && healthManager.playerHealth != null)
        {
            // Calculate the amount needed to reach max health
            int healingAmount = healthManager.playerHealth.MaxHp - healthManager.playerHealth.Hp;
            healthManager.Heal(healingAmount);  // Call Heal from HealthManager
        }
    }

    // Method to apply upgrade power-up effect
    public void ApplyUpgradePowerUp()
    {
        Weapon weapon = player.GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.IncreaseFireRate(1.5f, 20f); // Temporarily increase fire rate by 50% for 20 seconds
        }
    }
}
