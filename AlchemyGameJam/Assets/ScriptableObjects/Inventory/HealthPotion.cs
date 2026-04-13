using UnityEngine;

namespace ScriptableObjects.Inventory
{
    [CreateAssetMenu(menuName = "Game/Potions/Heal Potion")]
    public class HealthPotion : APotion
    {
        public int healAmount;
        public override void Use(GameObject user)
        {
            throw new System.NotImplementedException();
        }
    }
}