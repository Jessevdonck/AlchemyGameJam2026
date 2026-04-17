using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
