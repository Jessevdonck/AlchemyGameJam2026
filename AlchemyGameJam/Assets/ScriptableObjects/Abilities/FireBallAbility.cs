using UnityEngine;

namespace ScriptableObjects.Abilities
{
    public class FireBallAbility : AbilityBase
    {
        [SerializeField] GameObject _fireballPrefab;
        [SerializeField] private InputReader input;
        
        protected override void Use(GameObject user)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(input.MousePosition);
            Vector2 dir = (worldPos - (Vector2)user.transform.position).normalized;

            GameObject projectile = Instantiate(_fireballPrefab, user.transform.position, user.transform.rotation);
            
        }
    }
}