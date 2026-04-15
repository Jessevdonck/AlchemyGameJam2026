using System;
using GlobalManagers.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Abilities
{
    public class AbilitiesHUD : MonoBehaviour
    {
        [Header("UI References")] 
        public GameObject hudRoot;
        
        public Image abilityOne;
        public Image abilityTwo;
        public Image abilityThree;
        public Image abilityFour;
        [Header("Backgrounds")]
        public Image abilityOneFill;
        public Image abilityTwoFill;
        public Image abilityThreeFill;
        public Image abilityFourFill;
        [Header("Cooldowns")]
        public Image abilityOneCooldown;
        public Image abilityTwoCooldown;
        public Image abilityThreeCooldown;
        public Image abilityFourCooldown;
        
        public void UpdateDisplay(Sprite sprite, string itemName)
        {
            if (sprite == null)
            {
                ClearSlot();
                return;
            }

            hudRoot.SetActive(true);
            
        }

        public void ClearSlot()
        {
            
        }
    }
}