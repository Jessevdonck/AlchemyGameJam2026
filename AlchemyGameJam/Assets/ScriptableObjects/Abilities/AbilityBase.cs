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

        public Input input;
        
        public Timer _cooldownTimer;

        public float progressPercentage => _cooldownTimer.Progress;

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