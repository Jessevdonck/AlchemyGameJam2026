using UnityEngine;
using System;
using Interfaces;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Base Stats")]
    [SerializeField] private float baseMaxHealth = 100f;

    [SerializeField] private float currentHealth;
    private float maxHealth;

    // Deze staan hier al voor eventuele powerups/negatieve buffs later
    private float bonusHealth = 0f;
    private float damageMultiplier = 1f;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public event Action<float, float> OnHealthChanged; //Nodig voor UI te updaten
    public event Action OnDeath; //Alle dingen die gaan moeten gebeuren wanneer speler sterft

    private void Awake()
    {
        RecalculateMaxHealth();
        currentHealth = maxHealth;
    }

    public void RecalculateMaxHealth()
    {
        maxHealth = baseMaxHealth + bonusHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
    
    public void TakeDamage(float amount)
    {
        float finalDamage = amount * damageMultiplier;

        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log("Player Died");
        OnDeath?.Invoke();
    }
}