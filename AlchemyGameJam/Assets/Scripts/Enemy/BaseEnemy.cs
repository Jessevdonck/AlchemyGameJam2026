using System.Collections;
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
    [SerializeField] private float deathAnimDuration = 0.6f;

    private bool isDead = false;
    public Team team = Team.Enemy;

    public float spawnTime;
    protected Health health;
    private IEnemyTracker tracker;

    public void Init(IEnemyTracker enemyTracker)
    {
        tracker = enemyTracker;
        tracker.RegisterEnemy();
        spawnTime = Time.time;
    }

    protected virtual void Awake()
    {
        health = GetComponent<Health>();

        if (health != null)
            health.OnDeath += Die;

    }

    protected virtual void Die()
    {
        if (isDead) return; 

        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
        isDead = true;

        Debug.Log("DIE CALLED");

        var ai = GetComponent<EnemyAi>();
        if (ai != null)
            ai.enabled = false;

        var anim = GetComponent<EnemyAnimation>();
        if (anim != null)
            anim.PlayDeath();
    }
    
    private void OnDestroy()
    {
        Debug.Log("ENEMY DESTROYED");
    }
    
    public void OnDeathAnimationEnd()
    {
        DropLoot();
        tracker?.UnregisterEnemy();
        Destroy(gameObject);
    }
    
    public virtual void TakeDamage(float damage)
    {
        if (isDead) return; 

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
    
    public EnemyStats GetStats()
    {
        return stats;
    }
}