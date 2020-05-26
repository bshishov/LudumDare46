using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIWorkProgress : MonoBehaviour
    {
        public string Format = "N0";
        
        [Header("References")]
        public TextMeshProUGUI Name;
        public UIProgressBar ProgressBar;
        public UINumericText CurrentAmountLabel;
        public TextMeshProUGUI TargetAmountLabel;

        public void Setup(string optionName, float target, float current)
        {
            if (Name != null)
                Name.text = optionName;
            
            UpdateUI(target, current);
        }

        public void UpdateUI(float target, float current)
        {
            if(TargetAmountLabel != null)
                TargetAmountLabel.text = target.ToString(Format);
            
            if(CurrentAmountLabel != null)
                CurrentAmountLabel.SetValue(current);
            
            if(ProgressBar != null)
                ProgressBar.SetTarget(current / target);
        }
    }
}