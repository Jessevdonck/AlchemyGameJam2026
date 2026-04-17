using Player;
using Unity.Mathematics;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    [CreateAssetMenu(menuName = "Game/Abilities/Vortex")]
    public class VortexAbility : AbilityBase
    {
        public GameObject vortexPrefab;
        public float baseDamage;
        protected override void Use(GameObject user, Vector2 mousePos)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            GameObject vortex = Instantiate(vortexPrefab, worldPos, quaternion.identity);
            PlayerStats playerStats = user.GetComponent<PlayerStats>();
            playerStats.RecalculateStats();
            vortex.GetComponent<Vortex>().Init(playerStats.CalculateDamage(baseDamage), user);
        }
    }
}