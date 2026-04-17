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
    
    [Header("Audio")]
    [SerializeField] private AudioClip[] shootClips;
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private float pitchVariation = 0.1f;
    [SerializeField] private float volume = 1f;

    private AudioSource audioSource;
    private int lastShootIndex = -1;
    private int lastHitIndex = -1;

    // Set these in the Inspector — drag in your Animator and configure
    // two states: "Fly" (looping 2-5 frames) and "Hit" (one-shot)
    [Header("Hit Animation")]
    [Tooltip("How long the Hit animation lasts before the object is destroyed")]
    public float hitAnimDuration = 0.4f;

    private static readonly int FlyTrigger = Animator.StringToHash("Fly");
    private static readonly int HitTrigger = Animator.StringToHash("Hit");

    private ProjectileAnimation animHandler;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animHandler = GetComponent<ProjectileAnimation>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

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
        PlayRandomSound(shootClips, ref lastShootIndex);
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
            animHandler.PlayHit();

        yield return new WaitForSeconds(hitAnimDuration);
        Destroy(gameObject);
    }
    
    void PlayRandomSound(AudioClip[] clips, ref int lastIndex)
    {
        if (clips == null || clips.Length == 0 || audioSource == null) return;

        int index;
        do
        {
            index = Random.Range(0, clips.Length);
        }
        while (clips.Length > 1 && index == lastIndex);

        lastIndex = index;

        audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(clips[index], volume);
    }
}