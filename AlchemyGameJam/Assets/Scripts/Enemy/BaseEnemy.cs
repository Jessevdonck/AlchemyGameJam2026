using Interfaces;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] protected EnemyStats stats;

    protected Health health;
    private RoundManager waveManager;

    protected virtual void Awake()
    {
        health = GetComponent<Health>();

        if (health != null)
            health.OnDeath += Die;

        waveManager = FindObjectOfType<RoundManager>();
    }

    protected virtual void Die()
    {
        waveManager?.UnregisterEnemy();
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage)
    {
        if (health != null)
            health.TakeDamage(damage);
    }
}