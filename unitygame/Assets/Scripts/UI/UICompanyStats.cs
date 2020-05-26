using Gameplay;
using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    public class UICompanyStats : MonoBehaviour, IPopup
    {
        public float DestroyDelay => Fader != null ? Fader.FadeTime : 0f;
        
        [Header("References")]
        public UINumericText Balance;
        public TextMeshProUGUI OfficeLevel;
        public UITimer ExpensesTimer;
        public UICanvasGroupFader Fader;

        private Company _company;
        
        void Start()
        {
            StartCoroutine(CoroutineUtils.SlowUpdate(SlowUpdate, 0.5f));
        }

        private void SlowUpdate(float delay)
        {
            if(_company == null)
                return;
            
            if(Balance != null)
                Balance.SetTarget(_company.Balance);
            
            if(OfficeLevel != null)
                OfficeLevel.text = _company.Level.ToString();
        }

        public void Setup(Company company)
        {
            _company = company;

            if(_company != null)
            {
                if (ExpensesTimer != null)
                    ExpensesTimer.Timer = _company.ExpensesTimer;
            }
            
            // proc a single update
            SlowUpdate(0f);
        }
         
        public void OnShow()
        {
            if(Fader != null)
                Fader.FadeIn();
        }

        public void OnHide()
        {
            if(Fader != null)
                Fader.FadeOut();
        }
    }
}
