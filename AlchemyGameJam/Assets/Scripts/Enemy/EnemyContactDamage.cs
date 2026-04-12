using UnityEngine;

namespace Enemy
{
    public class EnemyContactDamage : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private float cooldown = 1f;

        private float timer;

        private void Update()
        {
            timer -= Time.deltaTime;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player") == false) return;

            if (timer > 0f) return;

            PlayerHealth player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(damage);
                timer = cooldown;
            }
        }
    }
}