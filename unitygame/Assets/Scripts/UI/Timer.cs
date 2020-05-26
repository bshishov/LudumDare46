using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        public float MaxTime = 120f;
        public float RemainingTime;
        
        [SerializeField]
        TextMeshProUGUI RemainingTimeLabel;

        public UIProgressBar ProgressBar;

        void Update()
        {
            if (RemainingTime > 0)
            {
                RemainingTime -= Time.deltaTime;
            }
            else
            {
                RemainingTime = 0;
            }
            
            if (ProgressBar != null)
                ProgressBar.SetTarget(RemainingTime / MaxTime);

            if (RemainingTimeLabel != null)
            {
                RemainingTimeLabel.text = Mathf.Floor(RemainingTime / 60).ToString("00");
                RemainingTimeLabel.text += ":" + (RemainingTime % 60).ToString("00");    
            }
        }
    }
}
