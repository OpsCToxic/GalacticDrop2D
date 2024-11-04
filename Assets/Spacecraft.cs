using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public GameObject alienPrefab;         // Alien prefab to be dropped
    public Transform player;               // Reference to the player
    public float flySpeed = 5f;            // Speed of spaceship's movement
    public float dropInterval = 1f;        // Time interval between each alien drop
    public Vector2 movementRange = new Vector2(-10f, 10f); // Left and right limits for spaceship movement

    private List<GameObject> activeAliens = new List<GameObject>(); // List to track active aliens
    private bool movingRight = true;       // Flag for movement direction

    void Start()
    {
        if (alienPrefab == null || player == null)
        {
            return; // Exit Start if alienPrefab or player is null to prevent errors
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
            // Check if there are no aliens left before spawning a new batch
            if (activeAliens.Count == 0)
            {
                yield return DropAlienBatch();
            }
            yield return null; // Wait until the next frame to recheck
        }
    }

    IEnumerator DropAlienBatch()
    {
        for (int i = 0; i < 3; i++)
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