using Gameplay.Lang;
using Gameplay.Voting;
using UnityEngine;

namespace UI.Popups
{
    public class MoneyVotingPopup : MonoBehaviour, IPopup
    {
        public float DestroyDelay => 0f;

        public SetupPopUp BasePopup;
        public UITimer Timer;
        public UIFundRaiser FundRaiser;

        private EventInfo _eventInfo;
        private MoneyVotingState _state;

        public void Setup(EventInfo eventInfo, MoneyVotingState state)
        {
            _state = state;
            _eventInfo = eventInfo;
            BasePopup.Setup(eventInfo.Title, eventInfo.Description);

            if (Timer != null)
                Timer.Timer = state.Timer;

            FundRaiser.Setup(state.Info.TargetAmount, state.CurrentAmount);
        }

        void Update()
        {
            if(_state == null)
                return;
            
            FundRaiser.Setup(_state.Info.TargetAmount, _state.CurrentAmount);
        }


        public void OnShow()
        {
        }

        public void OnHide()
        {
        }
    }
}