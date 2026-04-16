using UnityEngine;
using Player;
using ScriptableObjects.Inventory;

public class LootPickup : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private ResourceData resource;
    private PotionBase potion;
    private int amount;

    public void Setup(ResourceData res, PotionBase pot, int amt)
    {
        resource = res;
        potion = pot;
        amount = amt;
            
        if (resource != null && resource.icon != null)
        {
            spriteRenderer.sprite = resource.icon;
        }
        else if (potion != null && potion.icon != null)
        {
            spriteRenderer.sprite = potion.icon;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Inventory inv = other.GetComponent<Inventory>();

        if (inv == null) return;

        if (resource != null)
        {
            inv.AddResource(resource, amount);
        }

        if (potion != null)
        {
            inv.AddPotion(potion);
        }

        Destroy(gameObject);
    }
}