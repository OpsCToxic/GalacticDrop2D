using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private float timer;

    void Start()
    {

    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1.8f)
        {
            timer = 0;
            shoot();
        }
    }

    void shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
