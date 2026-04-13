using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class SelectedItemHUD : MonoBehaviour
    {
        [Header("UI References")]
        public Image itemIcon;
        public TextMeshProUGUI itemNameText;   // swap for Text if not using TMP
        public GameObject hudRoot; // the whole panel — assign SelectedItemHUD itself
        
        [Header("Settings")]
        public Sprite emptySlotSprite;         // optional: shown when nothing selected
        public Color filledSlotColor = Color.white;


        

        private void Awake()
        {
        }

        void Start()
        {
            
        }
        
        public void UpdateDisplay(Sprite sprite, string itemName)
        {
            if (sprite == null)
            {
                ClearSlot();
                return;
            }

            hudRoot.SetActive(true);
            itemIcon.sprite = sprite;
            itemIcon.color = filledSlotColor;
            itemNameText.text = itemName;
            
        }

        public void ClearSlot()
        {
            itemIcon.sprite = emptySlotSprite;
            itemNameText.text = "";
        }
    }
}
