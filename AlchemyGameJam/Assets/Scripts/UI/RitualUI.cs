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

// ── One script per card slot ──────────────────────────────────────────────────
public class RitualCardSlot : MonoBehaviour
{
    public Image    iconImage;
    public TMP_Text nameText;
    public TMP_Text descText;
    public TMP_Text rarityText;
    public Button   button;

    private RitualCard _card;
    private RitualUI   _ui;

    public void Setup(RitualCard card, RitualUI ui)
    {
        _card = card;
        _ui   = ui;

        if (iconImage)   iconImage.sprite  = card.icon;
        if (nameText)    nameText.text     = card.cardName;
        if (descText)    descText.text     = card.description;
        if (rarityText)  rarityText.text   = card.rarity.ToString();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => _ui.OnCardChosen(_card));
    }
}