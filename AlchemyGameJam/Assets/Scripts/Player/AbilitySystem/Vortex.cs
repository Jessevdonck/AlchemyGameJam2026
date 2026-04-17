using Enum;
using UnityEngine;

using Enum;
using Interfaces;
using System.Collections.Generic;
using GlobalManagers;
using GlobalManagers.Timer;
using UnityEngine;

public class Vortex : MonoBehaviour
{
    public float damagePerSecond { get; set; }

    private GameObject owner;
    private Team ownerTeam;

    private Animator animator;
    private Collider2D col;

    [Header("Vortex Settings")]
    public float duration = 5f;
    public float pullRadius = 4f;
    public float pullForce = 100f;
    
    private ProjectileAnimation animHandler;
    private Timer _vortexTimer;

    // Tracks enemies currently inside the vortex radius
    private readonly List<Rigidbody2D> _pulledBodies     = new();
    private readonly List<IDamageable> _pulledDamageables = new();

    private void Awake()
    {
        animator    = GetComponent<Animator>();
        animHandler = GetComponent<ProjectileAnimation>();
        col         = GetComponent<Collider2D>();
    }

    public void Init(float dps, GameObject shooter)
    {
        damagePerSecond = dps;
        owner           = shooter;

        var teamHolder = shooter.GetComponentInParent<TeamHolder>();
        ownerTeam = teamHolder.team;

        // Tick damage every second, destroy after duration
        _vortexTimer = TimerManager.Instance.Register(
            duration,
            onComplete: OnVortexExpired,
            onTick:     OnVortexTick
        );
    }

    // Called every frame by TimerManager via onTick (progress 0→1)
    private float _lastDamageProgress = 0f;

    private void OnVortexTick(float progress)
    {
        // Pull all bodies currently inside toward center
        Vector2 center = transform.position;
        foreach (var body in _pulledBodies)
        {
            if (body == null) continue;
            Vector2 dir = center - body.position;
            body.AddForce(dir.normalized * pullForce, ForceMode2D.Force);
        }

        // Deal damage every full second elapsed
        float elapsed = progress * duration;
        if (elapsed - _lastDamageProgress >= 1f)
        {
            _lastDamageProgress += 1f;
            DamageAll();
        }
    }

    private void DamageAll()
    {
        for (int i = _pulledDamageables.Count - 1; i >= 0; i--)
        {
            var target = _pulledDamageables[i];
            if (target == null)
            {
                _pulledDamageables.RemoveAt(i);
                _pulledBodies.RemoveAt(i);
                continue;
            }
            target.TakeDamage(damagePerSecond);
        }
    }

    private void OnVortexExpired()
    {
        if (col != null) col.enabled = false;

        if (animHandler != null)
            animHandler.PlayHit();

        Destroy(gameObject, 0.4f); // match your hitAnimDuration
    }

    // ── Collider callbacks to track who is inside ──────────────────────────

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == owner) return;

        var teamHolder = other.GetComponentInParent<TeamHolder>();
        if (teamHolder != null && teamHolder.team == ownerTeam) return;

        var body   = other.GetComponentInParent<Rigidbody2D>();
        var target = other.GetComponentInParent<IDamageable>();

        if (target == null) return;

        // Avoid duplicates (multi-collider enemies)
        if (!_pulledDamageables.Contains(target))
        {
            _pulledDamageables.Add(target);
            _pulledBodies.Add(body);

            // Immediate hit on entry
            target.TakeDamage(damagePerSecond);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var target = other.GetComponentInParent<IDamageable>();
        int idx    = _pulledDamageables.IndexOf(target);
        if (idx >= 0)
        {
            _pulledDamageables.RemoveAt(idx);
            _pulledBodies.RemoveAt(idx);
        }
    }

    private void OnDestroy()
    {
        if (_vortexTimer != null)
            TimerManager.Instance.Cancel(_vortexTimer);
    }
}
