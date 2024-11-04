using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 0f; // Adjust this to match your effect's duration

    void Start()
    {
        Destroy(gameObject, lifetime); // Automatically destroy the prefab after `lifetime` seconds
    }
}
