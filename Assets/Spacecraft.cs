using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public GameObject alienPrefab;             // Alien prefab to be dropped
    public GameObject bossPrefab;              // Boss prefab to be dropped every 20 enemies
    public Transform player;                   // Reference to the player
    public float flySpeed = 5f;                // Speed of spaceship's movement
    public float dropInterval = 0.5f;          // Time interval between each alien drop
    public Vector2 movementRange = new Vector2(-10f, 10f); // Left and right limits for spaceship movement

    private List<GameObject> activeAliens = new List<GameObject>(); // List to track active aliens
    private bool movingRight = true;           // Flag for movement direction
    private int totalAliensDropped = 0;        // Counter for the total number of aliens dropped
    private int aliensPerBatch = 5;            // Initial number of aliens per batch
    private int enemiesSinceLastBoss = 0;      // Counter to track number of enemies since last boss drop
    private const int additionalAlienThreshold = 3; // Number of total drops needed to increase batch size
    private const int bossSpawnThreshold = 20;      // Number of enemies to drop before spawning a boss

    void Start()
    {
        if (alienPrefab == null || bossPrefab == null || player == null)
        {
            return; // Exit Start if alienPrefab, bossPrefab, or player is null to prevent errors
        }

        StartCoroutine(AlienDropLoop()); // Start continuously dropping aliens
    }

    void Update()
    {
        MoveSpaceship();
    }

    void MoveSpaceship()
    {
        // Move the spaceship left and right within the specified range
        if (movingRight)
        {
            transform.position += Vector3.right * flySpeed * Time.deltaTime;
            if (transform.position.x >= movementRange.y) movingRight = false;
        }
        else
        {
            transform.position += Vector3.left * flySpeed * Time.deltaTime;
            if (transform.position.x <= movementRange.x) movingRight = true;
        }
    }

    IEnumerator AlienDropLoop()
    {
        while (true)
        {
            // Check if there are no aliens left before spawning a new batch or boss
            if (activeAliens.Count == 0)
            {
                // Check if it's time to drop a boss
                if (enemiesSinceLastBoss >= bossSpawnThreshold)
                {
                    DropBoss();
                    enemiesSinceLastBoss = 0; // Reset boss counter
                }
                else
                {
                    yield return DropAlienBatch();
                }
            }
            yield return null; // Wait until the next frame to recheck
        }
    }

    IEnumerator DropAlienBatch()
    {
        for (int i = 0; i < aliensPerBatch; i++)
        {
            if (alienPrefab == null)
            {
                yield break; // Exit the coroutine if alienPrefab is missing
            }

            // Instantiate an alien at the spaceship's position
            GameObject alien = Instantiate(alienPrefab, transform.position, Quaternion.identity);
            activeAliens.Add(alien); // Add the new alien to the active list

            // Subscribe to OnAlienDeath to handle removal before destruction
            Health alienHealth = alien.GetComponent<Health>();
            if (alienHealth != null)
            {
                alienHealth.OnAlienDeath += HandleAlienDeath; // Subscribe to OnAlienDeath event
            }

            yield return new WaitForSeconds(dropInterval);
        }

        // Increase the total alien drop count and check if we need to increase the batch size
        totalAliensDropped += aliensPerBatch;
        enemiesSinceLastBoss += aliensPerBatch; // Track total enemies for boss

        if (totalAliensDropped >= additionalAlienThreshold)
        {
            aliensPerBatch++;               // Increase the number of aliens per batch
            totalAliensDropped = 0;         // Reset the threshold counter but keep aliensPerBatch growing
        }
    }

    void DropBoss()
    {
        // Instantiate a boss at the spaceship's position
        GameObject boss = Instantiate(bossPrefab, transform.position, Quaternion.identity);
        activeAliens.Add(boss); // Add the boss to the active list

        // Subscribe to OnAlienDeath to handle removal before destruction
        Health bossHealth = boss.GetComponent<Health>();
        if (bossHealth != null)
        {
            bossHealth.OnAlienDeath += HandleAlienDeath; // Subscribe to OnAlienDeath event
        }

    }

    private void HandleAlienDeath(GameObject alien)
    {
        // Remove the alien from the list when it dies
        if (alien != null && activeAliens.Contains(alien))
        {
            activeAliens.Remove(alien);
        }
    }
}
