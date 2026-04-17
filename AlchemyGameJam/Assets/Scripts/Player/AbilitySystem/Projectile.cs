using Enum;
using Interfaces;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage { get; set; }

    private GameObject owner;
    private Team ownerTeam;

    private Animator animator;
    private Collider2D col;
    private Rigidbody2D rb;

    // Set these in the Inspector — drag in your Animator and configure
    // two states: "Fly" (looping 2-5 frames) and "Hit" (one-shot)
    [Header("Hit Animation")]
    [Tooltip("How long the Hit animation lasts before the object is destroyed")]
    public float hitAnimDuration = 0.4f;

    private static readonly int FlyTrigger = Animator.StringToHash("Fly");
    private static readonly int HitTrigger = Animator.StringToHash("Hit");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col       = GetComponent<Collider2D>();
        rb        = GetComponent<Rigidbody2D>();

        Destroy(gameObject, 5f);
    }

    public void Init(float dmg, GameObject shooter, Vector2 direction, float speed)
    {
        damage    = dmg;
        owner     = shooter;

        var teamHolder = shooter.GetComponentInParent<TeamHolder>();
        ownerTeam = teamHolder.team;

        if (rb != null)
            rb.linearVelocity = direction * speed;

        transform.right = direction;

        // Start the looping flight animation
        if (animator != null)
            animator.SetTrigger(FlyTrigger);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == owner) return;

        var teamHolder = other.GetComponentInParent<TeamHolder>();
        if (teamHolder != null && teamHolder.team == ownerTeam) return;

        IDamageable target = other.GetComponentInParent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
            StartCoroutine(PlayHitAndDestroy());
        }
    }

    private IEnumerator PlayHitAndDestroy()
    {
        // Stop moving and disable the collider so it doesn't re-trigger
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        if (col != null)
            col.enabled = false;

        // Play the hit animation
        if (animator != null)
            animator.SetTrigger(HitTrigger);

        yield return new WaitForSeconds(hitAnimDuration);
        Destroy(gameObject);
    }
}