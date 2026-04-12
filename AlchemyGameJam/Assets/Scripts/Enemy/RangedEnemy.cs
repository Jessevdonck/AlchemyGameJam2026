using UnityEngine;

public class RangedEnemy : EnemyAi
{
    public GameObject bulletPrefab;

    private float cooldown;

    protected override void TickAI()
    {
        cooldown -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= stats.attackRange && cooldown <= 0f)
        {
            Shoot();
            cooldown = stats.attackCooldown;
        }
    }

    void Shoot()
    {
        Vector2 dir = (player.position - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Projectile>().Init(dir, stats.damage);
    }
}