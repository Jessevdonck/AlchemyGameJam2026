using UnityEngine;

namespace ScriptableObjects.Inventory
{
    [CreateAssetMenu(menuName = "Game/Potions/Heal Potion")]
    public class HealPotion : APotion
    {
        public int healAmount;
        public override void Use(GameObject user)
        {
            throw new System.NotImplementedException();
        }
    }
}