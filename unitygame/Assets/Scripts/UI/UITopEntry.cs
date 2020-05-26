using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITopEntry : MonoBehaviour
    {
        public Image TargetImage;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Index;
        public UINumericText Balance;
        
        public void Setup(int index, string name, float balance, Color bgrColor, Color textColor)
        {
            if (Index != null)
            {
                Index.text = index.ToString();
                Index.color = textColor;
            }

            if (Name != null)
            {
                Name.text = name;
                Name.color = textColor;
            }

            if (Balance != null)
            {
                Balance.SetTarget(balance);
                Balance.Text.color = textColor;
            }

            if (TargetImage != null)
                TargetImage.color = bgrColor;
        }
    }
}