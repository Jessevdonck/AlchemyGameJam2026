using System;
using UnityEngine;

namespace ScriptableObjects.Inventory
{
    [CreateAssetMenu(menuName = "Game/Potions/Strength Potion")]
    public class StrengthPotionBase : PotionBase
    {
        public override void Use(GameObject user)
        {
            // set player TakeDamage to false;
            Debug.Log("strength potion");
        }
    }
}