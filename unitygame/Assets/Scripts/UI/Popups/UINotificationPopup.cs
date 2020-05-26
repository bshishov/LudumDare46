using Gameplay.Lang;
using UnityEngine;

namespace UI.Popups
{
    public class UINotificationPopup : MonoBehaviour, IPopup
    {
        public SetupPopUp BasePopup;
        public float DestroyDelay => 0f;

        public void Setup(string title, string description)
        {
            if (BasePopup != null)
            {
                BasePopup.Setup(title, description);
            }
        }

        public void Setup(EventInfo eventInfo)
        {
            Setup(eventInfo.Title, eventInfo.Description);
        }
        
        public void OnShow()
        {
        }

        public void OnHide()
        {
        }
    }
}