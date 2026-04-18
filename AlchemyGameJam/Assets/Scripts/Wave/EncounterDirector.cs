using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using Interfaces;
using Player;
using Random = UnityEngine.Random;

public class EncounterDirector : MonoBehaviour, IEnemyTracker
{
    [Header("Setup")]
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private List<EnemyStats> enemies;

    [Header("Scaling")]
    [SerializeField] private float baseBudget = 10f;
    [SerializeField] private float budgetGrowth = 1.2f;
    [SerializeField] private float baseEncounterDuration = 20f;
    [SerializeField] private float durationGrowth = 1.1f;

    [Header("Pressure")]
    [SerializeField] private int maxAliveEnemies = 12;
    [SerializeField] private float baseSpawnInterval = 0.5f;

    [Header("Pacing")] [SerializeField] private float encounterStartDelay = 7f;
    
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Despawn")]
    [SerializeField] private float maxDistanceFromPlayer = 25f;
    
    [Header("Respawn")]
    [SerializeField] private float respawnCooldown = 1f;

    private float lastRespawnTime;
    private float checkInterval = 1f;
    private float lastCheckTime;
    
    [SerializeField] private AnimationCurve pressureCurve; 

    private int aliveEnemies;
    private int encounterIndex;
    private float remainingBudget;

    public Action<int> OnRoundChanged;

    private void Start()
    {
        StartCoroutine(RunGame());
    }

    private void Update()
    {
        if (Time.time >= lastCheckTime + checkInterval)
        {
            CheckEnemyDistances();
            lastCheckTime = Time.time;
        }
    }
    
    void CheckEnemyDistances()
    {
        if (player == null) return;

        var enemiesInScene = FindObjectsOfType<BaseEnemy>();

        foreach (var enemy in enemiesInScene)
        {
            if (enemy == null) continue;

            float dist = Vector2.Distance(enemy.transform.position, player.position);

            if (Time.time - enemy.spawnTime < 2f)
                continue;

            if (aliveEnemies >= maxAliveEnemies)
                continue;

            if (dist > maxDistanceFromPlayer)
            {
                if (Time.time >= lastRespawnTime + respawnCooldown)
                {
                    RespawnEnemy(enemy);
                    lastRespawnTime = Time.time;
                }
            }
        }
    }

    void RespawnEnemy(BaseEnemy enemy)
    {
        EnemyStats stats = enemy.GetStats(); 

        if (stats == null)
        {
            Destroy(enemy.gameObject);
            return;
        }

        UnregisterEnemy();

        Destroy(enemy.gameObject);
        
        Transform closest = GetClosestSpawnPoint();

        GameObject newEnemy = Instantiate(stats.prefab, closest.position, Quaternion.identity);

        BaseEnemy newBase = newEnemy.GetComponent<BaseEnemy>();
        if (newBase != null)
        {
            newBase.Init(this);
        }
    }
    
    Transform GetClosestSpawnPoint()
    {
        Transform best = null;
        float bestDist = float.MaxValue;

        foreach (var point in spawner.GetSpawnPoints())
        {
            float dist = Vector2.Distance(point.position, player.position);

            if (dist < bestDist)
            {
                bestDist = dist;
                best = point;
            }
        }

        return best;
    }
    

    IEnumerator RunGame()
    {
        while (true)
        {
            encounterIndex++;

            OnRoundChanged?.Invoke(encounterIndex);
            
            float budget = baseBudget * Mathf.Pow(budgetGrowth, encounterIndex);
            
            yield return StartCoroutine(RunEncounter(budget));
            
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator RunEncounter(float budget)
    {
        remainingBudget = budget;

        float encounterDuration = baseEncounterDuration * Mathf.Pow(durationGrowth, encounterIndex);
        

        yield return new WaitForSeconds(encounterStartDelay);
        
        float startTime = Time.time;

        while (remainingBudget > 0 && (Time.time - startTime) < encounterDuration)
        {
            if (CanSpawn())
            {
                EnemyStats test = GetRandomEnemy(remainingBudget);

                if (test == null)
                {
                    remainingBudget = 0;
                }
                else
                {
                    float pressure = GetPressureFactor();
                    yield return StartCoroutine(SpawnBurst(pressure));
                }
            }

            yield return new WaitForSeconds(0.25f);
        }

        while (aliveEnemies > 0)
        {
            yield return null;
        }

        CollectAllLoot();
        
        RitualSystem.Instance.TriggerRitual();
    }
    
    void CollectAllLoot()
    {
        var playerInv = FindObjectOfType<Inventory>();
        if (playerInv == null) return;

        var pickups = FindObjectsOfType<LootPickup>();

        foreach (var loot in pickups)
        {
            loot.FlyTo(playerInv.transform);
        }
    }

    IEnumerator SpawnBurst(float pressure)
    {
        float burstMultiplier = Mathf.Clamp(1f + encounterIndex * 0.05f, 1f, 2f);

        int burstSize = Mathf.RoundToInt(
            Random.Range(2, 5) * pressure * burstMultiplier
        );

        for (int i = 0; i < burstSize; i++)
        {
            if (!CanSpawn()) yield break;

            EnemyStats enemy = GetRandomEnemy(remainingBudget);

            if (enemy == null)
            {
                remainingBudget = 0;
                yield break;
            }

            spawner.Spawn(enemy.prefab, this);
            remainingBudget -= enemy.cost;

            yield return new WaitForSeconds(
                GetScaledSpawnInterval() * Random.Range(0.6f, 1.2f)
            );
        }

        yield return new WaitForSeconds(Random.Range(0.8f, 1.8f));
    }

    float GetScaledSpawnInterval()
    {
        float t = Mathf.Clamp01(encounterIndex / 10f);
        float speedMultiplier = Mathf.Lerp(1f, 0.5f, t);
        return baseSpawnInterval * speedMultiplier;
    }

    float GetPressureFactor()
    {
        float ratio = (float)aliveEnemies / maxAliveEnemies;

        if (pressureCurve != null && pressureCurve.length > 0)
            return pressureCurve.Evaluate(ratio);
        
        return Mathf.Lerp(1.5f, 0.3f, ratio);
    }

    bool CanSpawn()
    {
        return aliveEnemies < maxAliveEnemies && remainingBudget > 0;
    }

    EnemyStats GetRandomEnemy(float budget)
    {
        EnemyStats chosen = null;
        int count = 0;

        foreach (var e in enemies)
        {
            if (e.cost <= budget)
            {
                count++;
                
                if (Random.Range(0, count) == 0)
                {
                    chosen = e;
                }
            }
        }

        return chosen;
    }

    public void RegisterEnemy()
    {
        aliveEnemies++;
    }

    public void UnregisterEnemy()
    {
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
    }
}