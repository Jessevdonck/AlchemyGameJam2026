using UnityEngine;

namespace ScriptableObjects.Abilities
{
    public abstract class AbilityBase : ScriptableObject
    {
        public string abilityName;
        public string description;
        public float cooldown;
        public Sprite icon;

        private float _lastUsedTime = -Mathf.Infinity;

        public bool IsOnCooldown => Time.time - _lastUsedTime < cooldown;
        public float CooldownRemaining => Mathf.Max(0, cooldown - (Time.time - _lastUsedTime));

        public void TryUse(GameObject user)
        {
            if (IsOnCooldown) return;
            _lastUsedTime = Time.time;
            Use(user);
        }

        protected abstract void Use(GameObject user);
    }
}