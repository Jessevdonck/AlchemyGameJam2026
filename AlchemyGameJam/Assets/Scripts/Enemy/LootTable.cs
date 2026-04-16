using System.Collections.Generic;
using Enemy;
using UnityEngine;
using ScriptableObjects.Inventory;

[CreateAssetMenu(menuName = "Game/Loot Table")]
public class LootTable : ScriptableObject
{
    public List<LootEntry> loot;
    [SerializeField, Range(0f, 1f)] private float dropChance = 1f;
    public LootEntry RollLoot()
    {
        if (Random.value > dropChance)
            return null;
        
        float totalWeight = 0f;

        foreach (var entry in loot)
        {
            totalWeight += entry.weight;
        }

        float roll = Random.Range(0, totalWeight);
        float current = 0f;

        foreach (var entry in loot)
        {
            current += entry.weight;

            if (roll <= current)
            {
                return entry;
            }
        }

        return null;
    }
}