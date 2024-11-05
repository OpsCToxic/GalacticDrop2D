using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 60f;
    public float fireRate = 0.5f; // Default time in seconds between shots
    private float defaultFireRate; // Store the original fire rate

    public AudioSource shootingAudioSource; // Reference to the AudioSource for shooting sound

    private Vector3 target;
    private float nextFireTime = 0f;

    private Coroutine fireRateCoroutine; // Reference to the active fire rate coroutine

    void Start()
    {
        defaultFireRate = fireRate; // Store the default fire rate
        Cursor.visible = true;
    }

    void Update()
    {
        target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector3 difference = target - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Vector2 direction = difference.normalized;
            FireBullet(direction, rotationZ);
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireBullet(Vector2 direction, float rotationZ)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, rotationZ));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;

        // Play shooting sound if AudioSource is available
        if (shootingAudioSource != null)
        {
            shootingAudioSource.Play();
        }
    }

    // Temporarily increases fire rate for a set duration
    public void IncreaseFireRate(float multiplier, float duration)
    {
        // Stop any existing fire rate coroutine to avoid overlap
        if (fireRateCoroutine != null)
        {
            StopCoroutine(fireRateCoroutine);
        }
        fireRateCoroutine = StartCoroutine(TemporarilyIncreaseFireRate(multiplier, duration));
    }

    private System.Collections.IEnumerator TemporarilyIncreaseFireRate(float multiplier, float duration)
    {
        fireRate /= multiplier; // Increase fire rate by dividing by the multiplier
        yield return new WaitForSeconds(duration); // Wait for the duration
        fireRate = defaultFireRate; // Reset fire rate to default
    }
}
