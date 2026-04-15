using System;
using Unity.VisualScripting;

namespace GlobalManagers.Timer
{
    public class Timer
    {
        public bool IsDone { get; private set; }
        public float Progress => 1f - (_remaining / _duration);

        private float _remaining;
        private readonly float _duration;
        private readonly Action _onComplete;
        private readonly Action<float> _onTick;  
        
        public float CooldownRemaining => _remaining;

        public Timer(float duration, Action onComplete, Action<float> onTick = null)
        {
            _duration   = duration;
            _remaining  = duration;
            _onComplete = onComplete;
            _onTick     = onTick;
        }

        public void Tick(float deltaTime)
        {
            if (IsDone) return;
            _remaining -= deltaTime;
            _onTick?.Invoke(Progress);

            if (_remaining <= 0)
            {
                _remaining = 0;
                IsDone = true;
                _onComplete?.Invoke();
            }
        }
    }
}