using Interfaces;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float damage = 10f;
    private GameObject owner;

    public void Init(Vector2 dir, float spd, GameObject shooter, float dmg = 10f)
    {
        direction = dir;
        speed = spd;
        owner = shooter;
        damage = dmg;

        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root == owner.transform.root) return;

        IDamageable target = other.GetComponentInParent<IDamageable>();

        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}