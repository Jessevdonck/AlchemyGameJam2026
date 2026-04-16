using ScriptableObjects.Inventory;
using UnityEngine;

namespace Enemy
{
    [System.Serializable]
    public class LootEntry
    {
        public ResourceData resource;
        public PotionBase potion;

        [Header("amount")] 
        public int minAmount = 0;
        public int maxAmount = 1;

        [Header("Weight")] public float weight = 1f;
    }
}