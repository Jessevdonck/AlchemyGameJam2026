using System;
using System.Collections.Generic;
using ScriptableObjects.Inventory;
using UI;
using UnityEngine;

namespace Player
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private SelectedItemHUD hud;
        [SerializeField] private InputReader input;

        private Dictionary<ResourceData, int> _resources = new();

        public List<PotionBase> potions = new();
        public int maxPotionSlots = 3;

        private int _selectedPotion = 0;
        
        public event Action<ResourceData, int> OnResourceChanged;

        private void Awake()
        {
            input.OnUsePotion += UsePotion;
            input.OnNextPotion += NextPotion;
            UpdateUi();
        }

        public void NextPotion()
        {
            if (potions.Count <= 1) return; 

            _selectedPotion = (_selectedPotion + 1) % potions.Count;
            UpdateUi();
        }

        public void PreviousPotion()
        {
            if (potions.Count == 0)
            {
                _selectedPotion = 0;
                return;
            }

            _selectedPotion = (_selectedPotion - 1 + potions.Count) % potions.Count;
            UpdateUi();
        }

        internal void UpdateUi()
        {
            if (potions.Count == 0)
            {
                _selectedPotion = 0;
                hud.ClearSlot();
                return;
            }

            _selectedPotion = Mathf.Clamp(_selectedPotion, 0, potions.Count - 1);

            var potion = potions[_selectedPotion];

            if (potion == null)
            {
                Debug.LogWarning("Null potion found, removing...");
                potions.RemoveAt(_selectedPotion);
                UpdateUi();
                return;
            }

            hud.UpdateDisplay(potion.icon, potion.potionName);
        }

        public void AddResource(ResourceData resource, int amount)
        {
            _resources.TryAdd(resource, 0);
            _resources[resource] += amount;
            
            OnResourceChanged?.Invoke(resource, _resources[resource]); 
        }

        public int GetResource(ResourceData resource)
        {
            return _resources.TryGetValue(resource, out var value) ? value : 0;
        }

        public bool TrySpendResource(ResourceData resource, int amount)
        {
            if (GetResource(resource) < amount)
                return false;

            _resources[resource] -= amount;
            
            OnResourceChanged?.Invoke(resource, _resources[resource]);
            
            return true;
        }

        public bool AddPotion(PotionBase potionBase)
        {
            if (potionBase == null) return false;
            if (potions.Count >= maxPotionSlots) return false;

            potions.Add(potionBase);
            UpdateUi();
            return true;
        }

        public void UsePotion()
        {
            if (potions.Count == 0) return;

            var potion = potions[_selectedPotion];

            if (potion == null)
            {
                potions.RemoveAt(_selectedPotion);
                UpdateUi();
                return;
            }

            potion.Use(gameObject);

            potions.RemoveAt(_selectedPotion);
            
            if (potions.Count == 0)
            {
                _selectedPotion = 0;
            }
            else if (_selectedPotion >= potions.Count)
            {
                _selectedPotion = potions.Count - 1;
            }

            UpdateUi();
        }
    }
}