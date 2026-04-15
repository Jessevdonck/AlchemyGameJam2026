using System;
using System.Collections.Generic;
using UnityEngine;


namespace GlobalManagers
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Instance { get; private set; }

        private readonly List<Timer.Timer> _timers = new List<Timer.Timer>();
        private readonly List<Timer.Timer> _toRemove = new List<Timer.Timer>();

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            foreach (var timer in _timers)
            {
                timer.Tick(Time.deltaTime);
                if (timer.IsDone) _toRemove.Add(timer);
            }

            foreach (var timer in _toRemove)
                _timers.Remove(timer);

            _toRemove.Clear();
        }

        public Timer.Timer Register(float duration, Action onComplete, Action<float> onTick = null)
        {
            var timer = new Timer.Timer(duration, onComplete, onTick);
            _timers.Add(timer);
            return timer;
        }

        public void Cancel(Timer.Timer timer)
        {
            _toRemove.Add(timer);
        }
    }
}