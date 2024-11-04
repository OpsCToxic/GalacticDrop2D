using System.Diagnostics;
using UnityEngine;

public class AIPath : MonoBehaviour
{
    private Rigidbody2D rb2D;

    // AI movement settings
    public float maxSpeed = 3.0f;
    public float acceleration = 5.0f;
    public float slowdownDistance = 0.6f;
    private GameObject target; // Reference to the target (Player)

    private Vector2 velocity2D;
    private bool isDead = false; // Flag to stop movement on death

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

        // Automatically find the player by tag
        target = GameObject.FindWithTag("Player");

    }

    void FixedUpdate()
    {
        // Stop movement if dead
        if (isDead)
        {
            rb2D.linearVelocity = Vector2.zero; // Ensure it stays still
            return;
        }

        // Follow target if the target is found
        if (target != null)
        {
            FollowTargetInX();
        }
    }

    void FollowTargetInX()
    {
        // Calculate the target position on the x-axis
        Vector2 targetPosition = new Vector2(target.transform.position.x, rb2D.position.y);

        // Calculate direction and distance to the target in x-axis only
        Vector2 direction = (targetPosition - rb2D.position).normalized;
        float distance = Mathf.Abs(target.transform.position.x - rb2D.position.x);

        // Calculate speed based on distance for smooth stopping near target
        float speed = distance < slowdownDistance ? Mathf.Lerp(0, maxSpeed, distance / slowdownDistance) : maxSpeed;

        // Compute desired velocity toward the target and apply acceleration
        Vector2 desiredVelocity = direction * speed;
        velocity2D = Vector2.MoveTowards(velocity2D, desiredVelocity, acceleration * Time.fixedDeltaTime);

        // Apply velocity to the Rigidbody2D for movement on the x-axis
        rb2D.linearVelocity = new Vector2(velocity2D.x, rb2D.linearVelocity.y);

        // Flip the alien's orientation based on the target's position
        if (target.transform.position.x < rb2D.position.x)
        {
            // Target is on the left side, face left
            transform.localScale = new Vector3(-0.5f, 0.4f, 0.5f); // Flip horizontally
        }
        else
        {
            // Target is on the right side, face right
            transform.localScale = new Vector3(0.5f, 0.4f, 0.5f); // Default orientation
        }
    }

    // Call this method from the Health script when the enemy dies
    public void OnDeath()
    {
        isDead = true;
    }

    // Optional: Reset isDead if the alien is reused or respawned
    public void ResetMovement()
    {
        isDead = false;
    }
}
