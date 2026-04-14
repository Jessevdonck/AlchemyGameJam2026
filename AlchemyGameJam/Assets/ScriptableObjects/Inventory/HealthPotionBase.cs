using System;
using UnityEngine;

namespace ScriptableObjects.Inventory
{
    [CreateAssetMenu(menuName = "Game/Potions/Heal Potion")]
    public class HealthPotionBase : PotionBase
    {
        public int healAmount;
        public override void Use(GameObject user)
        {
            Debug.Log("potion used");
        }
    }
}