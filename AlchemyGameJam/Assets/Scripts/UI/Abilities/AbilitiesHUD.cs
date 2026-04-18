using System;
using System.Collections.Generic;
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
            for (int i = 0; i < 3; i++)
            {
                if (i < abilities.Count)
                {
                    var ab = abilities[i];

                    // --- Icon ---
                    _abilityImages[i].sprite = ab.icon;
                    _abilityImages[i].color = Color.white;

                    // --- Cooldown overlay ---
                    var cd = _abilityCooldowns[i];

                    cd.sprite = ab.icon; // zelfde icon gebruiken
                    cd.color = new Color(0f, 0f, 0f, 0.8f); // donker overlay

                    cd.type = Image.Type.Filled;
                    cd.fillMethod = Image.FillMethod.Vertical;
                    cd.fillOrigin = (int)Image.OriginVertical.Bottom;

                    cd.fillAmount = 1f - ab.progressPercentage;
                }
                else
                {
                    _abilityImages[i].sprite = null;
                    _abilityImages[i].color = Color.clear;

                    _abilityCooldowns[i].sprite = null;
                    _abilityCooldowns[i].color = Color.clear;
                    _abilityCooldowns[i].fillAmount = 0f;
                }
            }
        }

        public void ClearSlot()
        {
            abilities.Clear();
        }
    }
}