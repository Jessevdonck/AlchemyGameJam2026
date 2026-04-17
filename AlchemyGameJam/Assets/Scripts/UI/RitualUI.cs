using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Gameplay;

public class RitualUI : MonoBehaviour
{
    [Header("Panel & Slots")]
    public GameObject      ritualPanel;
    public RitualCardSlot[] cardSlots;        // assign 3 in Inspector

    [Header("Optional header text")]
    public TMP_Text headerText;
    
    void Awake()
    {
        ritualPanel.SetActive(false);
    }
    
    public void ShowRitual(List<RitualCard> cards)
    {
        ritualPanel.SetActive(true);
        if (headerText) headerText.text = "Choose Your Blessing";

        for (int i = 0; i < cardSlots.Length; i++)
            cardSlots[i].Setup(cards[i], this);
    }

    public void OnCardChosen(RitualCard card)
    {
        ritualPanel.SetActive(false);
        RitualSystem.Instance.ApplyCard(card);
    }
}
