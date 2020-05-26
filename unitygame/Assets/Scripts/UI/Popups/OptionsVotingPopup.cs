using System.Collections.Generic;
using System.Linq;
using Gameplay.Lang;
using Gameplay.Voting;
using UnityEngine;

#if DEBUGGER
using Utils.Debugger;
#endif

namespace UI.Popups
{
    public class OptionsVotingPopup : MonoBehaviour, IPopup
    {
        public float DestroyDelay => 0f;

        public SetupPopUp BasePopup;
        public UITimer Timer;
        
        public Transform OptionsContainer;
        public GameObject OptionPrefab; 

        private EventInfo _eventInfo;
        private OptionsVotingState _state;
        
        private readonly Dictionary<string, UIOption> _options = new Dictionary<string, UIOption>();
        
        void Start()
        {
        }

        public void Setup(EventInfo eventInfo, OptionsVotingState state)
        {
            _state = state;
            _eventInfo = eventInfo;
            BasePopup.Setup(eventInfo.Title, eventInfo.Description);

            if (Timer != null)
                Timer.Timer = state.Timer;
                        
            ClearOptions();

            var idx = 1;
            foreach (var option in state.Options)
            {
                var uiOption = AddUiOption(idx++, option.Key, 0, option.Value.IsEnabled);
                _options.Add(option.Key, uiOption);
            }
        }

        void Update()
        {
            if(_state == null)
                return;

            var totalVotes = _state.Results.Values.Sum();
            foreach (var result in _state.Results)
            {
                var optionKey = result.Key;
                var uiOption = _options[optionKey];
                if (uiOption != null)
                {
                    uiOption.SetVotes(result.Value, totalVotes);
                }
            }
        }

        public void OnShow()
        {
        }

        public void OnHide()
        {
        }

        private void ClearOptions()
        {
            foreach (Transform children in OptionsContainer.transform)
                Destroy(children.gameObject);
            
            _options.Clear();   
        }

        private UIOption AddUiOption(int optionIndex, string optionName, int numVotes, bool isEnabled)
        {
            var go = GameObject.Instantiate(OptionPrefab, OptionsContainer);
            var option = go.GetComponent<UIOption>();
            option.Setup(optionIndex, optionName, numVotes, isEnabled);
            return option;
        }
    }
}