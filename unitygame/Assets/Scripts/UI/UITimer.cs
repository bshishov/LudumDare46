using System;
using Gameplay;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UITimer : MonoBehaviour
    {
        [Header("References")]
        public UIProgressBar ProgressBar;
        public TextMeshProUGUI RemainingSecondsLabel;

        public TimerState Timer;

        private void Update()
        {
            if (Timer != null)
            {
                if (ProgressBar != null)
                    ProgressBar.SetTarget(Timer.Progress);

                if (RemainingSecondsLabel != null)
                    RemainingSecondsLabel.text = FormatSeconds(Timer.TimeRemaining);
            }
        }

        private string FormatSeconds(float seconds)
        {
            var sTotal = Mathf.RoundToInt(seconds);
            var m = sTotal / 60;
            var s = sTotal % 60;
            return $"{m:D2}:{s:D2}";
        }
    }
}