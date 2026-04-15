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
        
        public Timer CooldownTimer;

        public float progressPercentage => CooldownTimer.Progress;

        public void TryUse(GameObject user, Vector2 mousePos)
        {
            if (CooldownTimer is { IsDone: false }) return;

            Use(user, mousePos);
            CooldownTimer = TimerManager.Instance.Register(cooldown, () =>
            {
                CooldownTimer = null;
            });
            
            
        }

        protected abstract void Use(GameObject user, Vector2 mousePos);
    }
}