using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public class UIProgressBar : MonoBehaviour
    {
        public float Initial = 1f;
        public float ChangeTime = 0.5f;

        private float _value;
        private float _target;
        private float _velocity;
        private Slider _slider;

        void Awake()
        {
            _slider = GetComponent<Slider>();
            _target = Initial;
            SetValue(Initial);
        }

        void Update()
        {
            SetValue(Mathf.SmoothDamp(_value, _target, ref _velocity, ChangeTime));
        }

        void SetValue(float val)
        {
            _value = val;
            _slider.value = _value;
        }

        public void SetTarget(float value)
        {
            _target = value;
        }

        [ContextMenu("Random target")]
        public void RandomTarget()
        {
            SetTarget(Random.value);
        }
    }
}