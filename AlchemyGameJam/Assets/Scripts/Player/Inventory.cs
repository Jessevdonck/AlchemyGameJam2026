using System;
using System.Collections.Generic;
using ScriptableObjects.Inventory;
using UnityEngine;

namespace Player
{
    public class Inventory : MonoBehaviour
    {
    
        public event Action OnInventoryChanged;
    
        private Dictionary<ResourceData, int> _resources = new();
    
        public List<APotion> potionSlots = new List<APotion>();
        public int maxPotionSlots = 5;
    
        public void AddResource(ResourceData resource, int amount)
        {
            if (!_resources.ContainsKey(resource))
                _resources[resource] = 0;

            _resources[resource] += amount;
            OnInventoryChanged?.Invoke();
        }

        public int GetResource(ResourceData resource)
        {
            return _resources.ContainsKey(resource) ? _resources[resource] : 0;
        }

        public bool AddPotion(APotion potion)
        {
            if (potionSlots.Count >= maxPotionSlots)
                return false;

            potionSlots.Add(potion);
            OnInventoryChanged?.Invoke();
            return true;
        }

        public void UsePotion(int index, GameObject user)
        {
            if (index < 0 || index >= potionSlots.Count) return;

            potionSlots[index].Use(user);
            potionSlots.RemoveAt(index);

            OnInventoryChanged?.Invoke();
        }
    }
}