using TMPro;
using UnityEngine;

namespace UI
{
    public class UIFundRaiser : MonoBehaviour
    {
        public string Format = "# ### ### ### ###";
        
        public UIProgressBar ProgressBar;
        public UINumericText CurrentAmountLabel;
        public TextMeshProUGUI TargetAmountLabel;

        public void Setup(float target, float current)
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