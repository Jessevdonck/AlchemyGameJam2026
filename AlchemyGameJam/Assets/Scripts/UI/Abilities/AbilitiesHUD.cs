using System;
using System.Collections.Generic;
using GlobalManagers.Timer;
using ScriptableObjects.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Abilities
{
    public class AbilitiesHUD : MonoBehaviour
    {
        [Header("UI References")] 
        [SerializeField] private Image[] _abilityImages;
        
        [SerializeField] private Image[] _abilityCooldowns;

        public List<AbilityBase> abilities;
        
        private void Update()
        {
            UpdateAbilities();
        }

        private void UpdateAbilities()
        {
            for(int i = 0; i < 4; i++)
            {
                if (i < abilities.Count)
                {
                    var ab = abilities[i];
                    _abilityImages[i].sprite = ab.icon;
                    _abilityImages[i].color = Color.white;
                    _abilityCooldowns[i].color = Color.white;
                    _abilityCooldowns[i].rectTransform.transform.localScale = new Vector3(1, 1 - ab.progressPercentage, 1);
                }
                else
                {
                    _abilityImages[i].color = Color.clear;
                    _abilityImages[i].sprite = null;
                    _abilityCooldowns[i].color = Color.clear;
                }
            }
        }
        

        public void ClearSlot()
        {
            
        }
    }
}