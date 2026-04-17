using Player;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    [CreateAssetMenu(menuName = "Game/Abilities/Fireball")]
    public class FireBallAbility : AbilityBase
    {
        public GameObject fireballPrefab;

        public int baseDamage;
        public int speed;
        
        protected override void Use(GameObject user, Vector2 mousePos)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 dir = (worldPos - (Vector2)user.transform.position).normalized;

            GameObject fireball = Instantiate(
                fireballPrefab,
                user.transform.position,
                Quaternion.identity
            );

            Projectile projectile = fireball.GetComponent<Projectile>();
            
            
            PlayerStats playerStats = user.GetComponent<PlayerStats>();
            projectile.Init(playerStats.CalculateDamage(baseDamage), user, dir, speed);

        }
    }
}