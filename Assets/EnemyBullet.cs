
using System.Security.Cryptography;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    public float force;
    public float speed = 20f;
    public GameObject impactEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Check if the object has a Health component
        Health targetHealth = hitInfo.GetComponent<Health>();
        if (targetHealth != null)
        {
            // Reduce health by 10
            targetHealth.Damage(10);
        }
        Instantiate(impactEffect, transform.position, transform.rotation);
        // Destroy the bullet after collision
        Destroy(gameObject);
    
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
