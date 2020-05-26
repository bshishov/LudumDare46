using TMPro;
using UnityEngine;

namespace UI
{
    public class UIOption : MonoBehaviour
    {
        public Color DisabledColor = Color.gray;
        
        [Header("References")]
        public TextMeshProUGUI OptionName;
        public TextMeshProUGUI NumOption;
        public TextMeshProUGUI NumVotes; 
        public UIProgressBar ProgressBar;

        public void Setup(int optionIndex, string optionName, int numVotes, bool isEnabled = true)
        {
            if (OptionName != null)
            {
                OptionName.text = optionName;
                if (!isEnabled)
                    OptionName.color = DisabledColor;
            }

            if(NumOption != null)
                NumOption.text = optionIndex.ToString();

            if (NumVotes != null)
            {
                NumVotes.text = numVotes.ToString();
                if (!isEnabled)
                    NumVotes.enabled = false;
            }

            if (ProgressBar != null)
            {
                ProgressBar.RandomTarget();
                if (!isEnabled)
                    ProgressBar.enabled = false;
            }
        }

        public void SetVotes(int numVotes, int maxVotes)
        {
            maxVotes = Mathf.Max(1, maxVotes);
            
            if(ProgressBar != null) 
                ProgressBar.SetTarget((float) numVotes / maxVotes);

            if (NumVotes != null)
                NumVotes.text = numVotes.ToString();
        }
    }
}