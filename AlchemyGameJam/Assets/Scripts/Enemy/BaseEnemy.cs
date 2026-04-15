using System;
using Interfaces;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] protected EnemyStats stats;

    protected Health health;

    protected virtual void Awake()
    {
        health = GetComponent<Health>();

        if (health != null)
            health.OnDeath += Die;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    
    public virtual void TakeDamage(float damage)
    {
        if (health != null)
        {
            health.TakeDamage(damage);  
        }
            
    }
}