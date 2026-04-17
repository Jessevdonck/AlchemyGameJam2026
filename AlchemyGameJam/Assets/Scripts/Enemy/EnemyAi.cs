using UnityEngine;

public abstract class EnemyAi : BaseEnemy
{
    protected Transform player;

    [Header("Separation")]
    [SerializeField] protected float separationRadius = 1.2f;
    [SerializeField] protected float separationStrength = 2f;
    [SerializeField] protected LayerMask enemyLayer;

    protected float decisionTimer;
    protected float decisionInterval = 0.3f;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Update()
    {
        if (!player) return;

        decisionTimer -= Time.deltaTime;

        if (decisionTimer <= 0f)
        {
            MakeDecision();
            decisionTimer = decisionInterval;
        }

        TickAI();
    }

    protected abstract void MakeDecision();
    protected abstract void TickAI();

    // ---------------- SEPARATION ----------------

    protected Vector2 GetSeparationForce()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            separationRadius,
            enemyLayer
        );

        Vector2 force = Vector2.zero;

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            EnemyAi other = hit.GetComponentInParent<EnemyAi>();
            if (other == null) continue;

            Vector2 diff = (Vector2)transform.position - (Vector2)other.transform.position;
            float dist = diff.magnitude;

            if (dist > 0)
            {
                force += diff.normalized / (dist * dist);
            }
        }

        return force * separationStrength;
    }
    protected void MoveWithSeparation(Vector2 direction)
    {
        Vector2 myPos = transform.position;

        Vector2 separation = GetSeparationForce();

        Vector2 finalDir = (separation * 2f + direction).normalized;

        transform.position = myPos + finalDir * stats.moveSpeed * Time.deltaTime;
    }
}