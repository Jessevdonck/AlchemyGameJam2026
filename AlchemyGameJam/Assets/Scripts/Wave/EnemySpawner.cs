using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    public void Spawn(GameObject prefab, RoundManager manager)
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        if (prefab == null)
        {
            Debug.LogError("Enemy prefab is null!");
            return;
        }

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(prefab, point.position, Quaternion.identity);

        manager.RegisterEnemy();
    }
}