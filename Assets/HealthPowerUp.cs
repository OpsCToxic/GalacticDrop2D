using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object colliding is the player
        if (other.CompareTag("Player"))
        {
            // Access the PowerUpManager to apply the health power-up effect
            PowerUpManager powerUpManager = FindObjectOfType<PowerUpManager>();
            if (powerUpManager != null)
            {
                powerUpManager.ApplyHealthPowerUp();
            }

            // Deactivate the power-up after collection
            gameObject.SetActive(false);
        }
    }
}
