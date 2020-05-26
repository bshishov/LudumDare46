using System.Collections.Generic;
using Gameplay;
using Gameplay.Lang;
using Gameplay.Voting;
using UnityEngine;

namespace UI.Popups
{
    public class UIContractPanel : MonoBehaviour, IPopup
    {
        public float DestroyDelay => Fader == null ? 0f : Fader.FadeTime;

        [Header("Prefabs")] 
        public GameObject WorkProgressPrefab;
        
        [Header("References")]
        public SetupPopUp BasePopup;
        public UITimer Timer;
        public RectTransform OptionsContainer;
        public UICanvasGroupFader Fader;
        
        private ContractState _state;
        private readonly Dictionary<string, UIWorkProgress> _progress = new Dictionary<string, UIWorkProgress>();

        public void Setup(ContractState state)
        {
            _state = state;
            BasePopup.Setup(state.Info.Name, state.Info.Description);

            if (Timer != null)
                Timer.Timer = state.WorkVotingState.Timer;

            ClearItems();
            
            foreach (var kvp in state.WorkVotingState.Progress)
                AddItem(kvp.Key, kvp.Value);
        }

        void Update()
        {
            if(_state == null)
                return;

            foreach (var kvp in _state.WorkVotingState.Progress)
            {
                if (_progress.ContainsKey(kvp.Key))
                {
                    var comp = _progress[kvp.Key];
                    comp.UpdateUI(kvp.Value.Option.TargetAmount, kvp.Value.CurrentAmount);
                }
            }
        }

        private void AddItem(string key, WorkVotingState.WorkProgress item)
        {
            var go = Instantiate(WorkProgressPrefab, OptionsContainer);
            var comp =  go.GetComponent<UIWorkProgress>();
            _progress.Add(key, comp);
            
            comp.Setup(item.Option.Name, item.Option.TargetAmount, item.CurrentAmount);
        }

        private void ClearItems()
        {
            _progress.Clear();
            foreach (Transform child in OptionsContainer)
            {
                if(child.GetComponent<UIWorkProgress>() != null)
                    Destroy(child.gameObject);
            }
        }

        public void OnShow()
        {
            Fader.FadeIn();
        }

        public void OnHide()
        {
            Fader.FadeOut();
        }
    }
}