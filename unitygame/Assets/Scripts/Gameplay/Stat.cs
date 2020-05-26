using UnityEngine;

namespace Gameplay
{
    public class Stat
    {
        public readonly string Name;
        public float Value;
        public readonly float MinValue;
        public readonly float MaxValue;

        public float Progress => Mathf.Clamp01((Value - MinValue) / (MaxValue - MinValue)); 

        public Stat(string name, float value, float minValue, float maxValue)
        {
            Name = name;
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}