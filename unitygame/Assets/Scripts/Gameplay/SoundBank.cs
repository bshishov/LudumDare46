using UnityEngine;
using Utils;

namespace Gameplay
{
    public class SoundBank : Singleton<SoundBank>
    {
        [Header("SFX")]
        public Sound PopupOpen;
        public Sound VoteReceived;
        public Sound ContractFinished;
        public Sound ContractStarted;
        public Sound ContractBoost;
        public Sound Bankruptcy;
        public Sound NewCompany;
        public Sound NewEvent;
        public Sound SalaryExpenses;
        public Sound OfficeUpgrade;
        public Sound MoneyEarned;
        
        [Header("Music")]
        public Sound BackgroundMusic;
        public Sound OfficeSoundsAmbience;
        
        private SoundManager.SoundHandler _musicHandler;
        private SoundManager.SoundHandler _officeAmbience;

        private void Start()
        {
            _musicHandler = SoundManager.Instance.Play(BackgroundMusic);
        }

        public void PlayOfficeAmbience()
        {
            _officeAmbience = SoundManager.Instance.Play(OfficeSoundsAmbience);
        }
        
        public void StopOfficeAmbience()
        {
            _officeAmbience?.Stop();
        }
    }
}