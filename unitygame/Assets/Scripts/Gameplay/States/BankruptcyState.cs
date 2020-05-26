using System.Collections.Generic;
using Gameplay.Lang;
using Gameplay.Lang.Votings;
using Gameplay.Voting;
using Twitch;
using UI;

namespace Gameplay.States
{
    public class BankruptcyState : BaseState
    {
        private MoneyVotingState _votingState;
        private EventInfo _event;
        
        public BankruptcyState(GameContext context, Client client, UserDb db) : base(context, client, db) {}

        public override void StateStarted()
        {
            /*
                1. Открываем MoneyVoting и ставим таймер                
            */
            _event = new EventInfo("Last chance", "Let's gather money to revive the company");
            var votingInfo = new MoneyVotingInfo(Common.BankruptcyGatherTarget - Context.Company.Balance);

            _votingState = new MoneyVotingState(votingInfo);
            UIManager.Instance.ShowMoneyVotingPopup(_event, _votingState);
            SoundManager.Instance.Play(SoundBank.Instance.Bankruptcy);
        }
        
        public override GameState? StateUpdate()
        {
            /*
             0. ** Обновляем UI голосования **
             1. Чекаем таймер голосования 
                если таймер законичлся
                    если сумму собрали - 
                        сумма переходит на баланс компании
                        переходим в Idle
                    
                    если сумму не собрали
                        переходим в CompanyStart              
             */
            if (_votingState == null) 
                return GameState.CompanyStart;
            
            _votingState.Update();
            if (_votingState.CanBeFinished())
            {
                _votingState.Finish(Context);
                if (_votingState.CurrentAmount < _votingState.Info.TargetAmount)
                {
                    // Fail
                    return GameState.CompanyStart;
                }

                // Saved
                Context.Company.Balance += _votingState.CurrentAmount;
                return GameState.Idle;
            }
            return null;
        }

        public override void StateEnded()
        {
            // скрываем окно голосования
            UIManager.Instance.HidePopup();
        }
        
        public override void OnUserMessage(User user, string message)
        {
            _votingState?.OnUserMessage(user, message);
        }

        public override void OnUserCommand(User user, string command, List<string> args)
        {
            _votingState?.OnUserCommand(user, command, args);
        }
    }
}