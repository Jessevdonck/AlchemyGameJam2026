using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjects.Inventory;

public class PotionCardUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image costSprite;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button brewButton;

    private PotionBase _potion;

    public void Setup(PotionBase potion, System.Action<PotionBase> onClick)
    {
        _potion = potion;

        title.text = potion.potionName;
        costText.text = $"{potion.costAmount}";
        icon.sprite = potion.icon;

        brewButton.onClick.RemoveAllListeners();
        brewButton.onClick.AddListener(() => onClick?.Invoke(_potion));
    }
}