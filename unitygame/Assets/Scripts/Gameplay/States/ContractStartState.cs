using System.Linq;
using Gameplay.Lang;
using Gameplay.Voting;
using Twitch;
using UI;
using Utils;
using Utils.FSM;

#if DEBUGGER
using UnityEngine;
using Utils.Debugger;
#endif

namespace Gameplay.States
{
    public class ContractStartState : BaseState
    {
        private NotificationVotingState _votingState;
        
        public ContractStartState(GameContext context, Client client, UserDb db) : base(context, client, db) { }

        public override void StateStarted()
        { 
            /*
                при условии что контракта активного нет
                
                выбрать рандомный контракт
                открыть popup с названием описанием и reward?      
                
                делаем его активным      
            */
            
            var availableContracts = Context.Contracts.Where(c => c.IsAvailable(Context));
            var selectedContract = RandomUtils.Choice(availableContracts.ToList());

            Debug.Log($"{selectedContract.Name} started {selectedContract.Description}");

            Context.Company.ActiveContract = new ContractState(selectedContract);
            
            _votingState = new NotificationVotingState(Common.InformationPopupDisplayTime);
            //var eventInfo = new EventInfo(selectedContract.Name, selectedContract.Description);
            var eventInfo = new EventInfo("New contract!",
                $"<b>{selectedContract.Name}</b>\n\n{selectedContract.Description}");
            UIManager.Instance.ShowNotificationPopup(eventInfo);
            SoundManager.Instance.Play(SoundBank.Instance.ContractStarted);
        }
        
        public override GameState? StateUpdate()
        {
            /*
                чилим Н секунд
                если таймер кончается - переходим в Idle
            */
            if (_votingState == null)
                return GameState.Idle;

            _votingState.Update();
            if (_votingState.CanBeFinished())
            {
                _votingState.Finish(Context);
                return GameState.Idle;
            }

            return null;
        }
        
        public override void StateEnded()
        {
            /*
               закрываем popup
            */
            UIManager.Instance.HidePopup();
        }
    }
}