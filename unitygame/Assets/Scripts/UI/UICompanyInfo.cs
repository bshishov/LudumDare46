using TMPro;
using UnityEngine;

namespace UI
{
    public class UICompanyInfo : MonoBehaviour
    {
        public UICanvasGroupFader CanvasGroupFader;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI CeoName;

        public void Setup(string companyName, string ceoName)
        {
            if (Name != null)
                Name.text = companyName;
            
            if (CeoName != null)
                CeoName.text = ceoName;
        }

        public void Show()
        {
            if (CanvasGroupFader != null)
            {
                CanvasGroupFader.FadeIn();
            }
        }
        
        public void Hide()
        {
            if (CanvasGroupFader != null)
            {
                CanvasGroupFader.FadeOut();
            }
        }
    }
}