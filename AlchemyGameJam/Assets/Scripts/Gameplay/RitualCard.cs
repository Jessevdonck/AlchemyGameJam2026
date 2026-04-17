using ScriptableObjects.Abilities;
using UnityEngine;
namespace Gameplay
{
    [CreateAssetMenu(fileName = "NewRitualCard", menuName = "Ritual/Card")]
    public class RitualCard : ScriptableObject
    {
        [Header("Display")]
        public string cardName = "Unnamed Card";
        [TextArea(2, 4)]
        public string description = "";
        public Sprite icon;
        public CardRarity rarity = CardRarity.Common;

        [Header("Card Type")]
        public CardType cardType = CardType.StatBuff;

        [Header("Flat Stat Buffs")]
        public float flatMaxHP;
        public float flatAttack;
        public float flatDefense;
        public float flatSpeed;
        public float flatDamageMulti;   // e.g. 0.2 = +0.2x damage

        [Header("Percentage Buffs  (0.1 = +10%)")]
        public float pctMaxHP;
        public float pctAttack;
        public float pctDefense;
        public float pctSpeed;
        public float pctDamageMulti;

        [Header("Ability (CardType.Ability or Mixed)")]
        public AbilityBase ability;   // matches your AbilityManager's ID

        [Header("Passive (CardType.Passive or Mixed)")]
        public PassiveEffectBase passivePrefab; // drag a prefab with the passive component
    }

    public enum CardType   { StatBuff, Ability, Passive, Mixed }
    public enum CardRarity { Common, Uncommon, Rare, Epic,Legendary }
}