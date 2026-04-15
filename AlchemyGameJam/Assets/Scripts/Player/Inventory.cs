using System;
using System.Collections.Generic;
using ScriptableObjects.Inventory;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private SelectedItemHUD hud;
        [SerializeField] private InputReader input;
        
        // private Health _health;
        
        private Dictionary<ResourceData, int> _resources = new();

        public List<PotionBase> potions;
        public int maxPotionSlots = 3;

        private int _selectedPotion = 0;

        private void Awake()
        {
            input.OnUsePotion += UsePotion;
            input.OnNextPotion += NextPotion;
            // _health = GetComponent<Health>();
            UpdateUi();
        }

        public void NextPotion()
        {
            if (potions.Count == 0)
            {
                _selectedPotion = 0;
                return;
            }
            _selectedPotion = (_selectedPotion + 1 + potions.Count) % potions.Count;
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
                hud.ClearSlot();
                return;
            }
            hud.UpdateDisplay(potions[_selectedPotion].icon, potions[_selectedPotion].potionName);
        }
        
        public void AddResource(ResourceData resource, int amount)
        {
            _resources.TryAdd(resource, 0);

            _resources[resource] += amount;
            
        }

        public int GetResource(ResourceData resource)
        {
            return _resources.TryGetValue(resource, out var value) ? value : 0;
        }

        public bool AddPotion(PotionBase potionBase)
        {
            if (potions.Count >= maxPotionSlots)
                return false;

            potions.Add(potionBase);
            
            return true;
        }
        
        public void UsePotion()
        {
            if(potions.Count == 0) return;
            Debug.Log(_selectedPotion);
            potions[_selectedPotion].Use(gameObject);
            potions.RemoveAt(_selectedPotion);
            PreviousPotion();
            Debug.Log(_selectedPotion);
            UpdateUi();
            Debug.Log(_selectedPotion);
        }
    }
}