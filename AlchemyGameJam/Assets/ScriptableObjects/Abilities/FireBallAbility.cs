using UnityEngine;

namespace ScriptableObjects.Abilities
{
    [CreateAssetMenu(menuName = "Game/Abilities/Fireball")]
    public class FireBallAbility : AbilityBase
    {
        public GameObject fireballPrefab;

        public int damage;
        public int speed;
        
        protected override void Use(GameObject user, Vector2 mousePos)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 dir = (worldPos - (Vector2)user.transform.position).normalized;
            
            var fireball = Instantiate(fireballPrefab, user.transform.position, user.transform.rotation);
            Projectile projectile = fireball.GetComponent<Projectile>();
            if (projectile != null) projectile.damage = damage;
            
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = dir * speed;
            
        }
    }
}