using UnityEngine;

public class HealPassive : PassiveEffectBase
{
    public float healAmount = 25f;

    public override void OnActivate()
    {
        var health = GetComponent<Health>();

        if (health != null)
        {
            health.Heal(healAmount);
        }
        else
        {
            Debug.LogWarning("No Health component found!");
        }

        Destroy(this); 
    }
}