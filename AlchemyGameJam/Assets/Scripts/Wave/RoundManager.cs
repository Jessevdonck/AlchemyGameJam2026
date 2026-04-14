using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private List<EnemyStats> enemies;

    [Header("Spawner")]
    [SerializeField] private EnemySpawner spawner;

    [Header("Scaling")]
    [SerializeField] private float baseBudget = 10f;
    [SerializeField] private float budgetGrowth = 3f;

    [Header("Wave Settings")]
    [SerializeField] private int minWaves = 2;
    [SerializeField] private int maxWaves = 5;
    [SerializeField] private float timeBetweenWaves = 3f;

    [Header("Spawning")]
    [SerializeField] private float spawnDelay = 0.5f;
    [SerializeField] private int maxAliveEnemies = 10;

    private int roundIndex = 0;
    private int aliveEnemies = 0;

    public event Action<int> OnRoundStart;
    public event Action<int> OnWaveStart;
    public event Action<int> OnWaveEnd;
    public event Action<int> OnRoundEnd;

    private void Start()
    {
        StartCoroutine(RunRounds());
    }

    IEnumerator RunRounds()
    {
        while (true)
        {
            roundIndex++;

            OnRoundStart?.Invoke(roundIndex);
            Debug.Log($"ROUND {roundIndex}");

            float roundBudget = baseBudget * Mathf.Pow(1.15f, roundIndex);

            int waveCount = Mathf.Clamp(
                2 + roundIndex / 2,
                minWaves,
                maxWaves
            );

            yield return StartCoroutine(RunRound(roundBudget, waveCount));

            while (aliveEnemies > 0)
                yield return null;

            Debug.Log("Round cleared!");
            OnRoundEnd?.Invoke(roundIndex);

            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator RunRound(float totalBudget, int waveCount)
    {
        float budgetPerWave = totalBudget / waveCount;

        for (int i = 0; i < waveCount; i++)
        {
            OnWaveStart?.Invoke(i + 1);
            Debug.Log($"Wave {i + 1}/{waveCount}");

            yield return StartCoroutine(SpawnWave(budgetPerWave));

            OnWaveEnd?.Invoke(i + 1);

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnWave(float budget)
    {
        float currentBudget = budget;

        while (currentBudget > 0)
        {
            if (aliveEnemies >= maxAliveEnemies)
            {
                yield return new WaitUntil(() => aliveEnemies < maxAliveEnemies);
            }

            EnemyStats enemy = GetRandomEnemy(currentBudget);
            if (enemy == null) yield break;

            spawner.Spawn(enemy.prefab, this);

            currentBudget -= enemy.cost;

            yield return SpawnBurstDelay();
        }
    }

    IEnumerator SpawnBurstDelay()
    {
        float burstPause = UnityEngine.Random.Range(0.3f, 0.8f);
        yield return new WaitForSeconds(burstPause);
    }

    EnemyStats GetRandomEnemy(float budget)
    {
        List<EnemyStats> possible = enemies.FindAll(e => e.cost <= budget);

        if (possible.Count == 0) return null;

        return possible[UnityEngine.Random.Range(0, possible.Count)];
    }

    public void RegisterEnemy() => aliveEnemies++;

    public void UnregisterEnemy()
    {
        aliveEnemies--;
        if (aliveEnemies < 0) aliveEnemies = 0;
    }
}