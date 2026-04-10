using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public float maxHealth = 100f;
    public float moveSpeed = 3f;
    public float damage = 10f;

    [Header("Ranged")]
    public float attackRange = 5f;
    public float attackCooldown = 2f;
}