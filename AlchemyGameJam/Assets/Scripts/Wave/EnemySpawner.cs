using UnityEngine;
using Interfaces;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    public void Spawn(GameObject prefab, IEnemyTracker tracker)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points!");
            return;
        }

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject obj = Instantiate(prefab, point.position, Quaternion.identity);

        BaseEnemy enemy = obj.GetComponent<BaseEnemy>();

        if (enemy != null)
        {
            enemy.Init(tracker);
        }
        else
        {
            Debug.LogWarning("Prefab has no BaseEnemy!");
        }
    }
}