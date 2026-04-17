using UnityEngine;
using ScriptableObjects.Inventory;
using Player;

[CreateAssetMenu(menuName = "Potions/Heal Potion")]
public class HealPotion : PotionBase
{
    public float healAmount = 25f;

    public override void Use(GameObject target)
    {
        var stats = target.GetComponent<PlayerStats>();

        if (stats != null)
        {
            stats.Heal(healAmount);
            Debug.Log("HEAL USED → " + stats.CurrentHP);
        }
        else
        {
            Debug.LogError("NO PLAYERSTATS FOUND");
        }
    }
}