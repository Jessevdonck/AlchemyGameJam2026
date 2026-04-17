using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ScriptableObjects.Inventory;
using Player;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private ResourceData resourceToTrack;

    [Header("UI")]
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Image iconImage;

    private void Start()
    {
        if (inventory != null)
        {
            inventory.OnResourceChanged += UpdateResource;
        }

        if (iconImage != null && resourceToTrack != null)
        {
            iconImage.sprite = resourceToTrack.icon;
        }

        UpdateResource(resourceToTrack, inventory.GetResource(resourceToTrack));
    }

    void UpdateResource(ResourceData resource, int amount)
    {
        if (resource != resourceToTrack) return;

        amountText.text = amount.ToString();
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnResourceChanged -= UpdateResource;
        }
    }
}