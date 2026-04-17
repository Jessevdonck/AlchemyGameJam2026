using System.Collections.Generic;
using GlobalManagers;
using Player;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    [CreateAssetMenu(menuName = "Game/Abilities/FireballCircle")]
    public class FireBallCircle : AbilityBase
    {
        public GameObject fireballPrefab;

        public int baseDamage;
        public int speed;

        private int fireballCount = 12;
        
        public float delayBetweenShots = 0.1f;
        protected override void Use(GameObject user, Vector2 mousePos)
        {
            PlayerStats playerStats = user.GetComponent<PlayerStats>();
            float damage = playerStats.CalculateDamage(baseDamage);
            float angleStep = 360f / fireballCount;
            int i = 0;

            TimerManager.Instance.Register(
                duration: delayBetweenShots * fireballCount,
                onComplete: null,
                onTick: _ =>
                {
                    if (i >= fireballCount) return;

                    // Check how many shots should have fired by now
                    int shouldHaveFired = Mathf.FloorToInt((i + 1));
                    while (i < shouldHaveFired && i < fireballCount)
                    {
                        float angle = i * angleStep;
                        Vector2 dir = new Vector2(
                            Mathf.Cos(angle * Mathf.Deg2Rad),
                            Mathf.Sin(angle * Mathf.Deg2Rad)
                        );
                        GameObject fireball = Instantiate(fireballPrefab, user.transform.position, Quaternion.identity);
                        fireball.GetComponent<Projectile>().Init(damage, user, dir, speed);
                        Debug.Log(dir);
                        i++;
                    }
                }
            );
        }
    }
}