using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

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
    
    [SerializeField] private AnimationCurve pressureCurve; 

    private int aliveEnemies;
    private int encounterIndex;
    private float remainingBudget;

    private void Start()
    {
        StartCoroutine(RunGame());
    }

    IEnumerator RunGame()
    {
        while (true)
        {
            encounterIndex++;

            float budget = baseBudget * Mathf.Pow(budgetGrowth, encounterIndex);
            

            yield return StartCoroutine(RunEncounter(budget));
            
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator RunEncounter(float budget)
    {
        remainingBudget = budget;

        float encounterDuration = baseEncounterDuration * Mathf.Pow(durationGrowth, encounterIndex);
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

        Debug.Log("Encounter Cleared!");
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