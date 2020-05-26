using System;
using Gameplay;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIDevelopmentStatus : MonoBehaviour
    {
        public Timer Timer;
        public UIProgressBar ProgressBar;
        public TextMeshProUGUI ContractName;
        public UICanvasGroupFader CanvasGroupFader;

        private ContractState _contract;

        public void Setup(ContractState contract)
        {
            _contract = contract;
            if (ContractName != null)
                ContractName.text = contract.Info.Name;

            if (Timer != null)
            {
                Timer.MaxTime = contract.WorkVotingState.Info.Duration;
                Timer.RemainingTime = contract.WorkVotingState.TimeRemaining;
            }
            
            Debug.LogWarning("UIDevelopmentStatus component is deprecated, dont use it");
        }

        public void Update()
        {
            if (_contract != null)
            {
                if (ProgressBar != null)
                    ProgressBar.SetTarget(_contract.WorkVotingState.Timer.ReverseProgress);

                if (Timer != null)
                    Timer.RemainingTime = _contract.WorkVotingState.TimeRemaining;
            }
        }

        public void Show()
        {
            if(CanvasGroupFader != null)
                CanvasGroupFader.FadeIn();
        }
        
        public void Hide()
        {
            if(CanvasGroupFader != null)
                CanvasGroupFader.FadeOut();
        }
    }
}