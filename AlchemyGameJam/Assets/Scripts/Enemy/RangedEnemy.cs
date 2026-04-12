using UnityEngine;

public class RangedEnemy : EnemyAi
{
    public GameObject bulletPrefab;

    private float cooldown;

    protected override void TickAI()
    {
        cooldown -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        float stopDistance = stats.attackRange;
        float retreatDistance = stats.attackRange * 0.5f;

        if (distance > stopDistance)
        {
            // te ver -> dichterbij komen
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                stats.moveSpeed * Time.deltaTime
            );
        }
        else if (distance < retreatDistance)
        {
            // te dichtbij -> weg bewegen
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                -stats.moveSpeed * Time.deltaTime
            );
        }

        // altijd schieten als mogelijk
        if (distance <= stopDistance && cooldown <= 0f)
        {
            Shoot();
            cooldown = stats.attackCooldown;
        }
    }

    void Shoot()
    {
        Vector2 dir = (player.position - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Projectile>().Init(dir, stats.damage, gameObject);
    }
}