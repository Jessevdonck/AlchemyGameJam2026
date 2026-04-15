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
        public TextMeshProUGUI itemNameText;  
        public GameObject hudRoot;
        
        [Header("Settings")]
        public Sprite emptySlotSprite;
        public Color filledSlotColor = Color.white;
        
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
