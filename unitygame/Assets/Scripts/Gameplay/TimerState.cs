using System;
using UnityEngine;

namespace Gameplay
{
    public class TimerState
    {
        public readonly float Duration;
        public float TimeRemaining;

        public float Progress => Mathf.Clamp01(TimeRemaining / Duration);
        public float ReverseProgress => 1f - Mathf.Clamp01(TimeRemaining / Duration);
        public bool IsActive => TimeRemaining > 0f;

        public event Action Finished;

        public TimerState(float duration)
        {
            Duration = duration;
            TimeRemaining = duration;
        }

        public void Update()
        {
            Update(Time.deltaTime);
        }

        public void Update(float dt)
        {
            if (TimeRemaining > 0)
            {
                TimeRemaining -= dt;
                if (TimeRemaining <= 0f)
                {
                    Finished?.Invoke();
                    TimeRemaining = 0f;
                }
            }
        }

        public void Reset()
        {
            TimeRemaining = Duration;
        }

        public void Skip()
        {
            TimeRemaining = 0;
        }
    }
}