using Player;
using UnityEngine;

public class AttackBuffPassive : PassiveEffectBase
{
    public float attackIncrease = 5f;

    public override void OnActivate()
    {
        if (stats == null)
            stats = GetComponent<PlayerStats>();

        stats.ApplyBuff(new Gameplay.RitualCard
        {
            flatAttack = attackIncrease
        });
    }
}