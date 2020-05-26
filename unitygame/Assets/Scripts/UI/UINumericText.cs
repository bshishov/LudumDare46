using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UINumericText : MonoBehaviour
    {
        public string Format = "N0";
        public float ChangeTime = 0.5f;
        public string ResultFormat = "{0}"; 

        public TextMeshProUGUI Text => _text;

        private TextMeshProUGUI _text;
        private float _value;
        private float _target;
        private float _velocity;
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            
            if (_text != null)
                _text.text = _value.ToString(Format);
        }

        private void Update()
        {
            SetValue(Mathf.SmoothDamp(_value, _target, ref _velocity, ChangeTime));
        }

        public void SetTarget(float value)
        {
            _target = value;
        }

        public void SetValue(float value)
        {
            if (_text != null)
                _text.text = string.Format(ResultFormat, _value.ToString(Format));
            _value = value;
        }
    }
}