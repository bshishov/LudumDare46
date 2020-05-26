using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay;
using Gameplay.Lang;
using Gameplay.Voting;
using Messages;
using UI.Popups;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("References")] 
        public Transform PopupParent;
        public UICanvasGroupFader PopupPanel;
        public UINumericText CompanyBalanceLabel;
        public UICompanyInfo CompanyInfo;
        public UITop Top;
        public AlternativeMoneyParticles alternativeMoneyParticles;
        [FormerlySerializedAs("UIContractPanel")] public UIContractPanel ContractPanel;
        public UICompanyStats CompanyStats;

        [Header("Prefabs")] 
        public GameObject MoneyVotingPopup;
        public GameObject OptionsVotingPopup;
        public GameObject DualVotingPopup;
        public GameObject ContractCompletedPopup;
        public GameObject NotificationPopup;

        private GameObject _activePopup;
        private Company _company;

        private void Start()
        {
            if(Top != null)
                Top.Setup(10);

            EventBus.Subscribe<ActiveContractChanged>(OnActiveContractChanged);
            EventBus.Subscribe<CompanyChanged>(OnCompanyChanged);
            EventBus.Subscribe<CompanyBalanceChanged>(OnCompanyBalanceChanged);
        }

        private void OnCompanyBalanceChanged(CompanyBalanceChanged obj)
        {
            var newBalance = obj.Content;
            if (CompanyBalanceLabel != null)
                CompanyBalanceLabel.SetTarget(newBalance);
            alternativeMoneyParticles.SetMoney(newBalance);
        }

        private void OnCompanyChanged(CompanyChanged obj)
        {
            _company = obj.Content;
            
            if (_company != null)
            {
                if (CompanyInfo != null)
                {
                    CompanyInfo.Show();
                    CompanyInfo.Setup(_company.Name, _company.Ceo?.Name);
                }

                if (CompanyStats != null)
                {
                    CompanyStats.Setup(_company);
                    CompanyStats.OnShow();    
                }
            }
            else
            {
                if(CompanyInfo != null)
                    CompanyInfo.Hide();
                
                if(CompanyStats != null)
                    CompanyStats.OnHide();
            }
        }

        private void OnActiveContractChanged(ActiveContractChanged obj)
        {
            var contract = obj.Content;
            if (ContractPanel != null)
            {
                if (contract != null)
                {
                    ContractPanel.Setup(contract);
                    ContractPanel.OnShow();
                }
                else
                {
                    ContractPanel.OnHide();
                }
            }
        }

        public MoneyVotingPopup ShowMoneyVotingPopup(EventInfo eventInfo, MoneyVotingState state)
        {
            var popupObj = ShowPopup(MoneyVotingPopup);
            var moneyVotingPopup = popupObj.GetComponent<MoneyVotingPopup>();
            moneyVotingPopup.Setup(eventInfo, state);
            return moneyVotingPopup;
        }

        public OptionsVotingPopup ShowOptionsVotingPopup(EventInfo eventInfo, OptionsVotingState state)
        {
            var popupObj = ShowPopup(OptionsVotingPopup);
            var popup = popupObj.GetComponent<OptionsVotingPopup>();
            popup.Setup(eventInfo, state);
            return popup;
        }

        public UIDualVotingPopup ShowDualOptionsVotingPopup(EventInfo eventInfo, DistrVotingState state)
        {
            var popupObj = ShowPopup(DualVotingPopup);
            var popup = popupObj.GetComponent<UIDualVotingPopup>();
            popup.Setup(eventInfo, state);
            return popup;
        }

        public UINotificationPopup ShowNotificationPopup(EventInfo eventInfo)
        {
            var popupObj = ShowPopup(NotificationPopup);
            var popup = popupObj.GetComponent<UINotificationPopup>();
            popup.Setup(eventInfo);
            return popup;
        }

        public GameObject ShowPopup(GameObject prefab)
        {
            if (prefab == null)
            {
                Debug.LogWarning("Prefab is not set");
                return null;
            }

            HidePopup();

            var go = Instantiate(prefab, PopupParent);
            var popup = go.GetComponent<IPopup>();
            popup?.OnShow();

            _activePopup = go;

            PopupPanel.FadeIn();

            SoundManager.Instance.Play(SoundBank.Instance.PopupOpen);
            return go;
        }

        public void HidePopup()
        {
            if (_activePopup == null)
                return;

            var popup = _activePopup.GetComponent<IPopup>();
            if (popup != null)
            {
                popup.OnHide();
                Destroy(_activePopup, popup.DestroyDelay);
            }
            else
            {
                Destroy(_activePopup);
            }

            PopupPanel.FadeOut();
        }

        public GameObject ShowContractCompletedPopup()
        {
            return ShowPopup(ContractCompletedPopup);
        }

        public void SetTopUsers(IEnumerable<User> users)
        {
            if (Top != null)
            {
                var idx = 0;
                foreach (var user in users)
                {
                    Top.Setup(idx++, user.Name, user.Balance);
                }
            }
        }
    }
}