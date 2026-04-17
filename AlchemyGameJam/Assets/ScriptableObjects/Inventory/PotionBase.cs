using UnityEngine;

namespace ScriptableObjects.Inventory
{
    public abstract class PotionBase : ScriptableObject
    {
        public string potionName;
        public Sprite icon;
        
        [Header("Cost")]
        public ResourceData costResource;
        public int costAmount = 1;

        public abstract void Use(GameObject user);
    }
}
