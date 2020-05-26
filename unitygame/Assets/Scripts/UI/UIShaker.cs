using UnityEngine;

namespace UI
{
    public class UIShaker : MonoBehaviour
    {
        private Vector3 _initialLocalPos;

        private float _shakeValue;
        [Range(0, 50f)] public float ShakeAmplitude = 50f;
        [Range(0.001f, 10f)] public float ShakeDecay = 1f;
        public RectTransform TargetTransform;

        private void Start()
        {
            if (TargetTransform == null)
                TargetTransform = GetComponent<RectTransform>();

            if (TargetTransform == null)
            {
                Debug.LogError("[UIShaker] Target transform is null. Turning off the shaker component");
                enabled = false;
                return;
            }

            _initialLocalPos = TargetTransform.localPosition;
        }

        private void Update()
        {
            _shakeValue = Mathf.Clamp01(_shakeValue - ShakeDecay * Time.deltaTime);
            var d = Mathf.Pow(_shakeValue, 3);
            if (d > 0f)
                TargetTransform.localPosition = _initialLocalPos +
                                                new Vector3(
                                                    d * Random.Range(-ShakeAmplitude, ShakeAmplitude),
                                                    d * Random.Range(-ShakeAmplitude, ShakeAmplitude), 0);
        }
        
        public void Shake(float force = 1f)
        {
            _shakeValue += force;
        }
    }
}