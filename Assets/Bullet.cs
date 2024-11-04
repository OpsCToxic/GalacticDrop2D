using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject impactEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Check if the object has a Health component
        Health targetHealth = hitInfo.GetComponent<Health>();
        if (targetHealth != null)
        {
            // Reduce health by 10
            targetHealth.Damage(25);
        }
        Instantiate(impactEffect, transform.position, transform.rotation);
        // Destroy the bullet after collision
        Destroy(gameObject);
    }

}
