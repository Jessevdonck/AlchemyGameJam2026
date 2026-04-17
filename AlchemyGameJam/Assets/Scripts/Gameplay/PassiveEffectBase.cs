using Player;
using UnityEngine;

/// <summary>
/// Derive from this to create any passive. Attach instances to a GameObject
/// (the player, or a dedicated PassiveHolder) — RitualSystem activates them.
/// </summary>
public abstract class PassiveEffectBase : MonoBehaviour
{
    [SerializeField] public PlayerStats stats; // swap for your combat class

    public virtual void OnActivate() { }   // called once when the passive is granted
    public virtual void OnDeactivate() { } // called if you ever remove it

    // ── Hooks — override whichever you need ──────────────────────────────────
    public virtual void OnDealDamage(float damageDealt, GameObject target) { }
    public virtual void OnKill(GameObject target) { }
    public virtual void OnTakeDamage(float damageReceived) { }
    public virtual void OnLevelUp() { }
}