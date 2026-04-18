using UnityEngine;
using Player;
using ScriptableObjects.Inventory;

public class LootPickup : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private ResourceData resource;
    private PotionBase potion;
    private int amount;

    private Transform target;
    private float moveSpeed = 10f;
    private bool isFlyingToPlayer = false;
    
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
    
    private void Update()
    {
        if (!isFlyingToPlayer || target == null) return;

        transform.position = Vector3.Lerp(
            transform.position,
            target.position,
            Time.deltaTime * moveSpeed
        );

        moveSpeed += Time.deltaTime * 15f; 
        
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            Collect();
        }
    }

    private void Collect(Inventory inv = null)
    {
        if (inv == null)
            inv = FindObjectOfType<Inventory>();

        if (resource != null)
            inv.AddResource(resource, amount);

        if (potion != null)
            inv.AddPotion(potion);

        Destroy(gameObject);
    }
    
    public void FlyTo(Transform player)
    {
        target = player;
        isFlyingToPlayer = true;

        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Inventory inv = other.GetComponent<Inventory>();

        if (inv == null) return;
        
       Collect(inv);
    }
}