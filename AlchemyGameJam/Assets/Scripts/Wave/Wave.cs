using UnityEngine;

[CreateAssetMenu(menuName = "Game/Wave")]
public class Wave : ScriptableObject
{
    public EnemySpawnData[] enemies;
}

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public int amount;
    public float spawnDelay;
}