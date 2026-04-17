using Player;
using UnityEngine;

public class HealPassive : PassiveEffectBase
{
    public float healAmount = 25f;

    public override void OnActivate()
    {
        if (stats == null)
            stats = GetComponent<PlayerStats>();

        stats.Heal(healAmount);
    }
}