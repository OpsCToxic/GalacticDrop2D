using System.Diagnostics;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject healthPowerUpPrefab;      // Reference to the existing inactive health power-up
    public GameObject upgradePowerUpPrefab;     // Reference to the existing inactive upgrade power-up
    public Transform player;                    // Reference to the player
    private HealthManager healthManager;        // Reference to the HealthManager

    public AudioSource heal; // Play heal sound
    public AudioSource upgrade; // Play upgrade sound
    public AudioSource death; // Plays death sound of enemy

    private int healthPowerUpKillCount = 0;     // Kill count for health power-up
    private int upgradePowerUpKillCount = 0;    // Kill count for upgrade power-up
    private const int healthKillsPerPowerUp = 9;  // Number of kills for health power-up
    private const int upgradeKillsPerPowerUp = 15; // Number of kills for upgrade power-up

    void Start()
    {
        // Find the HealthManager component in the scene and assign it
        healthManager = FindObjectOfType<HealthManager>();

        // Ensure power-ups are inactive at the start
        if (healthPowerUpPrefab != null) healthPowerUpPrefab.SetActive(false);
        if (upgradePowerUpPrefab != null) upgradePowerUpPrefab.SetActive(false);
    }

    // Called each time an enemy is killed
    public void OnEnemyKilled(Vector3 position)
    {
        death.Play();


        // Increment kill counters for both power-ups
        healthPowerUpKillCount++;
        upgradePowerUpKillCount++;

        // Check if it's time to activate a health power-up every 3 kills
        if (healthPowerUpKillCount >= healthKillsPerPowerUp)
        {
            ActivateHealthPowerUp(position);
            healthPowerUpKillCount = 0; // Reset health power-up counter
        }

        // Check if it's time to activate an upgrade power-up every 21 kills
        if (upgradePowerUpKillCount >= upgradeKillsPerPowerUp)
        {
            ActivateUpgradePowerUp(position);
            upgradePowerUpKillCount = 0; // Reset upgrade power-up counter
        }
    }

    // Method to activate and position the health power-up at the given position
    private void ActivateHealthPowerUp(Vector3 position)
    {
        if (healthPowerUpPrefab != null)
        {
            healthPowerUpPrefab.transform.position = position;
            healthPowerUpPrefab.SetActive(true); // Activate the health power-up
        }
    }

    // Method to activate and position the upgrade power-up at the given position
    private void ActivateUpgradePowerUp(Vector3 position)
    {
        if (upgradePowerUpPrefab != null)
        {
            upgradePowerUpPrefab.transform.position = position + new Vector3(1, 0, 0); // Offset the position
            upgradePowerUpPrefab.SetActive(true); // Activate the upgrade power-up
        }
    }

    // Method to apply health power-up effect using HealthManager's Heal method
    public void ApplyHealthPowerUp()
    {
        if (healthManager != null && healthManager.playerHealth != null)
        {
            
            // Heal the player to max health
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
