using Enum;
using Interfaces;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage { get; set; }

    private GameObject owner;
    private Team ownerTeam;

    public void Init(float dmg, GameObject shooter, Vector2 direction, float speed)
    {
        damage = dmg;
        owner = shooter;

        var teamHolder = shooter.GetComponentInParent<TeamHolder>();
        ownerTeam = teamHolder.team;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }

        transform.right = direction;
    }

    private void Awake()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == owner) return;

        var teamHolder = other.GetComponentInParent<TeamHolder>();

        if (teamHolder != null && teamHolder.team == ownerTeam)
        {
            return;
        }

        IDamageable target = other.GetComponentInParent<IDamageable>();

        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}