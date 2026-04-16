using Interfaces;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage { get; set; }
    
    public void Awake()
    {
        Destroy(gameObject, 5f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.CompareTag("Player")) return;
        IDamageable target = other.GetComponentInParent<IDamageable>();

        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}