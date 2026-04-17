using Gameplay;

namespace Player
{
    using UnityEngine;

    public class PlayerStats : MonoBehaviour
    {
        [Header("Base Stats")] public float baseMaxHP = 100f;
        public float baseAttack = 10f;
        public float baseDefense = 5f;
        public float baseSpeed = 5f;
        public float baseDamageMulti = 1f; // e.g. 1.0 = 100% damage
        public float baseCooldownHaste = 1f;

        public Health playerHealth;

        // ── Computed (read-only outside this class) ──────────────────────────────
        public float MaxHP { get; private set; }
        public float CurrentHP { get; private set; }
        public float Attack { get; private set; }
        public float Defense { get; private set; }
        public float Speed { get; private set; }
        public float DamageMulti { get; private set; }
        public float CooldownHaste { get; private set; }

        // ── Accumulated buffs ─────────────────────────────────────────────────────
        private float _flatHP, _flatAtk, _flatDef, _flatSpd, _flatDmgMul;
        private float _pctHP, _pctAtk, _pctDef, _pctSpd, _pctDmgMul, _pctCDHaste;

        void Awake()
        {
            RecalculateStats();
            CurrentHP = MaxHP;
            
                
        }

        public void RecalculateStats()
        {
            MaxHP = (baseMaxHP + _flatHP) * (1f + _pctHP);
            Attack = (baseAttack + _flatAtk) * (1f + _pctAtk);
            Defense = (baseDefense + _flatDef) * (1f + _pctDef);
            Speed = (baseSpeed + _flatSpd) * (1f + _pctSpd);
            DamageMulti = (baseDamageMulti + _flatDmgMul) * (1f + _pctDmgMul);
            CooldownHaste = baseCooldownHaste * (1f +_pctCDHaste);
        }

        public void ApplyBuff(RitualCard card)
        {
            // Flat
            _flatHP += card.flatMaxHP;
            _flatAtk += card.flatAttack;
            _flatDef += card.flatDefense;
            _flatSpd += card.flatSpeed;
            _flatDmgMul += card.flatDamageMulti;

            // Percentage
            _pctHP += card.pctMaxHP;
            _pctAtk += card.pctAttack;
            _pctDef += card.pctDefense;
            _pctSpd += card.pctSpeed;
            _pctDmgMul += card.pctDamageMulti;

            float oldMax = MaxHP;
            RecalculateStats();

            // Heal the difference if max HP increased
            float hpGain = MaxHP - oldMax;
            if (hpGain > 0) CurrentHP = Mathf.Min(CurrentHP + hpGain, MaxHP);
        }

        public float CalculateDamage(float rawDamage)
        {
            return rawDamage * DamageMulti;
        }
    


    /// <summary>Returns actual damage dealt after defense reduction.</summary>
    public void TakeDamage(float rawDamage)
    {
        float reduced = Mathf.Max(1f, rawDamage * (1f - Defense));
        playerHealth.TakeDamage(reduced);
    }

    public void Heal(float amount) =>
        CurrentHP = Mathf.Min(CurrentHP + amount, MaxHP);

    public bool IsAlive => CurrentHP > 0f;
}
}