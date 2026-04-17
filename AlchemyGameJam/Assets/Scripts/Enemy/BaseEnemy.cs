using Enemy;
using Enum;
using Interfaces;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] protected EnemyStats stats;
    [SerializeField] private LootTable lootTable;
    [SerializeField] private GameObject lootPickupPrefab;
    [SerializeField] private int lootRolls = 1; //aantal drops

    public Team team = Team.Enemy;
    
    protected Health health;
    private IEnemyTracker tracker;

    public void Init(IEnemyTracker enemyTracker)
    {
        tracker = enemyTracker;
        tracker.RegisterEnemy();
    }

    protected virtual void Awake()
    {
        health = GetComponent<Health>();

        if (health != null)
            health.OnDeath += Die;

    }

    protected virtual void Die()
    {
        DropLoot();
        tracker?.UnregisterEnemy();
        Destroy(gameObject);
    }
    
    public virtual void TakeDamage(float damage)
    {
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }

    void DropLoot()
    {
        for (int i = 0; i < lootRolls; i++)
        {
            LootEntry drop = lootTable.RollLoot();
            if (drop == null) continue;
            
            int amount = Random.Range(drop.minAmount, drop.maxAmount + 1);

            Vector2 offset = Random.insideUnitCircle * 0.5f;
            
            GameObject obj = Instantiate(lootPickupPrefab, transform.position, Quaternion.identity);
            
            LootPickup pickup = obj.GetComponent<LootPickup>(); 
            pickup.Setup(drop.resource, drop.potion, amount);
        }
    }
}