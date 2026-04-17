using UnityEngine;

public class RangedEnemy : EnemyAi
{
    private enum State
    {
        Approach,
        Strafe,
        Retreat,
        Attack
    }

    private State currentState;

    [Header("Combat")]
    public GameObject bulletPrefab;

    private float cooldown;

    private Vector2 strafeDirection;

    private float preferredRange;

    protected override void Start()
    {
        base.Start();

        preferredRange = stats.attackRange + Random.Range(-1.5f, 1.5f);
    }

    protected override void MakeDecision()
    {
        Vector2 myPos = transform.position;
        Vector2 playerPos = player.position;

        float distance = Vector2.Distance(myPos, playerPos);

        float optimalRange = preferredRange;
        float tooClose = preferredRange * 0.6f;

        if (Random.value < 0.02f)
        {
            preferredRange = stats.attackRange + Random.Range(-1.5f, 1.5f);
        }

        if (distance > optimalRange)
        {
            currentState = State.Approach;
        }
        else if (distance < tooClose)
        {
            currentState = State.Retreat;
        }
        else
        {
            strafeDirection = Random.value > 0.5f ? Vector2.left : Vector2.right;
            currentState = State.Strafe;
        }

        if (cooldown <= 0f && distance <= optimalRange)
        {
            currentState = State.Attack;
        }
    }

    protected override void TickAI()
    {
        cooldown -= Time.deltaTime;

        switch (currentState)
        {
            case State.Approach:
                MoveTowards(player.position);
                break;

            case State.Retreat:
                MoveAway(player.position);
                break;

            case State.Strafe:
                Strafe();
                break;

            case State.Attack:
                Attack();
                break;
        }
    }

    // ---------------- MOVEMENT ----------------

    void MoveTowards(Vector2 target)
    {
        Vector2 myPos = transform.position;
        Vector2 dir = (target - myPos).normalized;

        MoveWithSeparation(dir);
    }

    void MoveAway(Vector2 target)
    {
        Vector2 myPos = transform.position;
        Vector2 dir = (myPos - target).normalized;

        MoveWithSeparation(dir);
    }

    void Strafe()
    {
        Vector2 myPos = transform.position;
        Vector2 playerPos = player.position;

        Vector2 toPlayer = (playerPos - myPos).normalized;

        Vector2 perpendicular = new Vector2(-toPlayer.y, toPlayer.x);
        Vector2 orbit = toPlayer * 0.4f;
        Vector2 jitter = Random.insideUnitCircle * 0.1f;

        Vector2 dir = (perpendicular * strafeDirection.x + orbit + jitter).normalized;

        MoveWithSeparation(dir);
    }

    // ---------------- ATTACK ----------------

    void Attack()
    {
        if (cooldown > 0f) return;

        Shoot();

        cooldown = stats.attackCooldown + Random.Range(-0.2f, 0.2f);
    }

    void Shoot()
    {
        Vector2 myPos = transform.position;
        Vector2 playerPos = player.position;

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        Vector2 futurePos = playerPos;

        if (playerRb != null)
        {
            futurePos += playerRb.linearVelocity * 0.2f;
        }

        Vector2 dir = (futurePos - myPos).normalized;

        dir += Random.insideUnitCircle * 0.05f;
        dir.Normalize();

        GameObject bullet = Instantiate(bulletPrefab, myPos, Quaternion.identity);

        Projectile proj = bullet.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Init(stats.damage, gameObject, dir, 10f);
        }
    }
}