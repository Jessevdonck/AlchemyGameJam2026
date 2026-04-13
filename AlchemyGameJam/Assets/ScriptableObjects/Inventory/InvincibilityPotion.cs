using System;
using UnityEngine;

namespace ScriptableObjects.Inventory
{
    [CreateAssetMenu(menuName = "Game/Potions/Invincibility Potion")]
    public class InvincibilityPotion : APotion
    {
        public override void Use(GameObject user)
        {
            // set player TakeDamage to false;
            Debug.Log("invicibility potion");
        }
    }
}