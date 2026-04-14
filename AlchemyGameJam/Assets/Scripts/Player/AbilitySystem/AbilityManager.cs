using System;
using System.Collections.Generic;
using ScriptableObjects.Abilities;
using UnityEngine;

namespace Player.AbilitySystem
{
    public class AbilityManager : MonoBehaviour
    {
        [SerializeField] private List<AbilityBase> _abilities;
        
        [SerializeField] private InputReader input;
        
        private Action _useAbilityOne;
        private Action _useAbilityTwo;
        private Action _useAbilityThree;
        private Action _useAbilityFour;
        
        private void UseAbility(int index)
        {
            GetAbility(index)?.TryUse(gameObject);
        }

        private void Awake()
        {
            _useAbilityOne   = () => UseAbility(0);
            _useAbilityTwo   = () => UseAbility(1);
            _useAbilityThree = () => UseAbility(2);
            _useAbilityFour  = () => UseAbility(3);

            input.OnUseAbilityOne   += _useAbilityOne;
            input.OnUseAbilityTwo   += _useAbilityTwo;
            input.OnUseAbilityThree += _useAbilityThree;
            input.OnUseAbilityFour  += _useAbilityFour;
        }

        private void OnDestroy()
        {
            input.OnUseAbilityOne   -= _useAbilityOne;
            input.OnUseAbilityTwo   -= _useAbilityTwo;
            input.OnUseAbilityThree -= _useAbilityThree;
            input.OnUseAbilityFour  -= _useAbilityFour;
        }

        public AbilityBase GetAbility(int index)
        {
            return _abilities.Count < index + 1 ? null : _abilities[index];
        }
    }
}