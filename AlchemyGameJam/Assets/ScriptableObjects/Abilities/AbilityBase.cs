using GlobalManagers;
using GlobalManagers.Timer;
using UnityEngine;

namespace ScriptableObjects.Abilities
{
    public abstract class AbilityBase : ScriptableObject
    {
        public string abilityName;
        public string description;
        public float cooldown;
        public Sprite icon;

        public Timer _cooldownTimer;

        public void TryUse(GameObject user)
        {
            if (_cooldownTimer is { IsDone: false }) return;
                
            Use(user);
            _cooldownTimer = TimerManager.Instance.Register(cooldown, () =>
            {
                _cooldownTimer = null;
            });
            
            
        }

        protected abstract void Use(GameObject user);
    }
}