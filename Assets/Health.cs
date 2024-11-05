using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _maxHp = 100;

    public int _curHp;
    private Animator _animator;
    private PowerUpManager powerUpManager;

    private bool _isDead = false;

    public int MaxHp => _maxHp;

    // Event to notify when the alien is about to be destroyed
    public event Action<GameObject> OnAlienDeath;

    public int Hp
    {
        get => _curHp;
        private set
        {
            var isDamage = value < _curHp;
            _curHp = Mathf.Clamp(value, 0, _maxHp);

            // Trigger appropriate events for healing or damage
            if (isDamage)
            {
                Damaged?.Invoke(_curHp);
            }
            else
            {
                Healed?.Invoke(_curHp);
            }

            // Check if the object should die
            if (_curHp <= 0 && !_isDead)
            {
                _isDead = true;
                Dead?.Invoke();
                OnAlienDeath?.Invoke(gameObject); // Notify about impending destruction
            }
        }
    }

    public UnityEvent<int> Healed;
    public UnityEvent<int> Damaged;
    public UnityEvent Dead;
    public UnityEvent<int> OnHealthChanged;

    void Start()
    {
        _curHp = _maxHp;
        _animator = GetComponent<Animator>();
        powerUpManager = FindObjectOfType<PowerUpManager>();

        // Subscribe to Dead event
        Dead.AddListener(OnDeath);
    }

    public void Damage(int amount)
    {
        if (_isDead) return; // Do not allow further damage if dead
        Hp -= amount;
        OnHealthChanged?.Invoke(_curHp); // Update health UI or other elements
    }

    public void Heal(int amount)
    {
        if (_isDead) return; // Do not allow healing if dead
        Hp += amount;
    }

    private void OnDeath()
    {
        // Notify AIPath to stop movement
        AIPath aiPath = GetComponent<AIPath>();
        if (aiPath != null)
        {
            aiPath.OnDeath(); // Stop movement in AIPath script
        }
        KillCounter killCounter = FindObjectOfType<KillCounter>();
        if (killCounter != null)
        {
            killCounter.IncrementKillCount();
        }
        powerUpManager.OnEnemyKilled(transform.position);
        // Trigger death animation if there's an Animator
        if (_animator != null)
        {
            _animator.SetTrigger("Die");
            StartCoroutine(DestroyAfterAnimation()); // Destroy after animation completes
        }
        else
        {
            DestroyImmediately();
        }
    }

    private System.Collections.IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        DestroyImmediately();
    }

    // Separate destruction logic for flexibility
    private void DestroyImmediately()
    {
        Destroy(gameObject);
    }
}
