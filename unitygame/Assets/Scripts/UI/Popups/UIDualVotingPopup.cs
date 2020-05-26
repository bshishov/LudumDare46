using System;
using System.Linq;
using Gameplay.Lang;
using Gameplay.Voting;
using TMPro;
using UnityEngine;

namespace UI.Popups
{
    public class UIDualVotingPopup : MonoBehaviour, IPopup
    {
        public float DestroyDelay => 0f;
        
        public SetupPopUp BasePopup;
        public UITimer Timer;

        public UIProgressBar ProgressBar;
        public TextMeshProUGUI Option1Name;
        public TextMeshProUGUI Option2Name;
        public TextMeshProUGUI Option1Votes;
        public TextMeshProUGUI Option2Votes;
        
        private EventInfo _eventInfo;
        private DistrVotingState _state;
        private string _option1;
        private string _option2;
        
        public void OnShow()
        {
        }
        
        public void Setup(EventInfo eventInfo, DistrVotingState state)
        {
            _state = state;
            _eventInfo = eventInfo;
            BasePopup.Setup(eventInfo.Title, eventInfo.Description);

            if (Timer != null)
                Timer.Timer = state.Timer;
            
            if(state.Options.Count != 2)
                throw new InvalidOperationException("Only votes with 2 options are supported by this component");

            var optionNames = state.Options.Keys.ToArray();
            _option1 = optionNames[0];
            _option2 = optionNames[1];

            if (Option1Name != null)
                Option1Name.text = _option1;
            
            if (Option2Name != null)
                Option2Name.text = _option2;
        }
        
        void Update()
        {
            if(_state == null)
                return;

            var option1Votes = _state.Results[_option1];
            var option2Votes = _state.Results[_option2];
            var totalVotes = option1Votes + option2Votes;

            if (Option1Votes != null)
                Option1Votes.text = option1Votes.ToString();
            
            if (Option2Votes != null)
                Option2Votes.text = option2Votes.ToString();

            if (ProgressBar != null)
            {
                var progress = 0.5f;
                if (totalVotes >= 1)
                {
                    progress = (float) option1Votes / totalVotes;
                }
                
                ProgressBar.SetTarget(progress);
            }

        }

        public void OnHide()
        {
        }
    }
}