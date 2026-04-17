using UnityEngine;
using System.Collections.Generic;
using Gameplay;
using Player;
using Player.AbilitySystem;

namespace Gameplay
{
    public class RitualSystem : MonoBehaviour
{
    public static RitualSystem Instance { get; private set; }

    [Header("References")]
    public PlayerStats  playerStats;
    public RitualUI     ritualUI;
    public Transform    passiveHolder;  // usually the Player GameObject

    // Replace with your actual ability manager type:
    public AbilityManager abilityManager;

    [Header("Card Pool")]
    public List<RitualCard> commonCards;
    public List<RitualCard> uncommonCards;
    public List<RitualCard> rareCards;
    public List<RitualCard> legendaryCards;

    [Header("Rarity Weights (must sum to 1)")]
    [Range(0,1)] public float weightCommon    = 0.55f;
    [Range(0,1)] public float weightUncommon  = 0.30f;
    [Range(0,1)] public float weightRare      = 0.12f;
    [Range(0,1)] public float weightLegendary = 0.03f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ── Public entry points ───────────────────────────────────────────────────

    /// <summary>Call from altar, level-up, checkpoint, etc.</summary>
    public void TriggerRitual()
    {
        var offered = PickThreeCards();
        if (offered.Count < 3)
        {
            Debug.LogWarning("[RitualSystem] Not enough cards in the pool.");
            return;
        }
        Time.timeScale = 0f;
        ritualUI.ShowRitual(offered);
    }

    /// <summary>Called by RitualUI once the player clicks a card.</summary>
    public void ApplyCard(RitualCard card)
    {
        Time.timeScale = 1f;

        // Stat buffs
        playerStats.ApplyBuff(card);

        // Ability
        if ((card.cardType == CardType.Ability || card.cardType == CardType.Mixed))
        {
            abilityManager?.GetAbility(card.ability);
        }

        // Passive
        if ((card.cardType == CardType.Passive || card.cardType == CardType.Mixed)
             && card.passivePrefab != null)
        {
            var passive = passiveHolder.gameObject
                              .AddComponent(card.passivePrefab.GetType()) as PassiveEffectBase;
            if (passive != null)
            {
                passive.stats  = playerStats;
                // passive.combat = playerCombat; // wire if needed
                passive.OnActivate();
            }
        }

        Debug.Log($"[Ritual] Applied card: {card.cardName}");
    }

    // ── Card picking ──────────────────────────────────────────────────────────

    private List<RitualCard> PickThreeCards()
    {
        var all  = BuildWeightedPool();
        var picked = new List<RitualCard>();
        var used   = new HashSet<RitualCard>();

        int attempts = 0;
        while (picked.Count < 3 && attempts < 100)
        {
            var card = all[Random.Range(0, all.Count)];
            if (used.Add(card)) picked.Add(card);
            attempts++;
        }
        return picked;
    }

    private List<RitualCard> BuildWeightedPool()
    {
        var pool = new List<RitualCard>();
        AddWithWeight(pool, commonCards,    weightCommon);
        AddWithWeight(pool, uncommonCards,  weightUncommon);
        AddWithWeight(pool, rareCards,      weightRare);
        AddWithWeight(pool, legendaryCards, weightLegendary);
        return pool;
    }

    private void AddWithWeight(List<RitualCard> pool, List<RitualCard> source, float weight)
    {
        // Each card appears in the pool proportionally to its rarity weight
        int copies = Mathf.Max(1, Mathf.RoundToInt(weight * 100));
        foreach (var card in source)
            for (int i = 0; i < copies; i++)
                pool.Add(card);
    }
}
}