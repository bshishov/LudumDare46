using System.Collections.Generic;
using Gameplay.Lang;
using Gameplay.Lang.Votings;
using Gameplay.Voting;
using Twitch;
using UI;
using UnityEngine;

namespace Gameplay.States
{
    public class ContractEndState : BaseState
    {
        private const string CompanyOption = "Company";
        private const string InvestorsOption = "Workers";
        
        private DistrVotingState _votingState;
        private EventInfo _event;
        private float _reward;

        public ContractEndState(GameContext context, Client client, UserDb db) : base(context, client, db) {}
        
        public override void StateStarted()
        {
            /*
             1. открываем UI голосовалки распределения доходов (и начинаем принимать голоса)
                    в UI пишем reward или в чат?
                начинаем таймер голосования  
                
             2. выполняем SuccessAction    
             3. Context.Company.ActiveContract.Earnings = GetRewardAction()? 
             */
            if(!Context.Company.HasContract)
                return;
            
            var activeContract = Context.Company.ActiveContract;
            activeContract.Info.SuccessAction?.Call(Context);

            var options = new OptionsVotingInfo.Option[]
            {
                new OptionsVotingInfo.Option {Name = CompanyOption},
                new OptionsVotingInfo.Option {Name = InvestorsOption},
            };
            var votingInfo = new OptionsVotingInfo(true, options, Common.DefaultVotingDuration);
            _votingState = new DistrVotingState(votingInfo);
            _event = new EventInfo(
                "Split the money!", 
                $"You got an <b>$ {Context.Company.ActiveContract.Info.Reward:N0}</b> for finishing your last contract. Divide the money between the workers and the company - part of the earned money will go to the company's account, and part will be credited to your personal balance.");

            _reward = Context.Company.ActiveContract.Info.Reward;
            Context.Company.ActiveContract = null;
            UIManager.Instance.ShowDualOptionsVotingPopup(_event, _votingState);
            SoundManager.Instance.Play(SoundBank.Instance.ContractFinished);
        }

        public override GameState? StateUpdate()
        {
            /*
             0. ** обновляем UI голосования **
             1. обновляем таймер голосования
                если таймер (голосование) закончился
                    распределяем деньги (Context.Company.ActiveContract.Earnings) за контракт:
                    в зависимости от результатов голосования
                    c_Investors vs c_Company (percent)
                    Company.Balance += c_Company * Earnings
                    
                    for each user in active users:
                        user balance increase by c_Investors * Earnings                        
                    
                    ** не делим на количество человек
                        
                    смотрим счетчик контрактов (кол-во до события)
                    если счетчик достиг Х
                        переходим в EventState
                    иначе
                        переходим в ContractStart                    
            */
            if (_votingState == null)
                return GameState.Idle;
            
            _votingState.Update();
            if (_votingState.CanBeFinished())
            {
                _votingState.Finish(Context);
                var earnings = _reward;
                var companyVotes = _votingState.Results[CompanyOption];
                var investorsVotes = _votingState.Results[InvestorsOption];
                var total = companyVotes + investorsVotes;
                var toCompany = 0.5f;
                if (total > 0)
                    toCompany = (float)companyVotes / total;

                var toUsers = Mathf.Clamp01(1f - toCompany);

                SoundManager.Instance.Play(SoundBank.Instance.MoneyEarned);
                Context.Company.Balance += earnings * toCompany;
                // TODO: выплаты только тем, кто участвовал в контракте
                foreach (var activeUser in Context.Db.GetActiveUsers())
                    activeUser.Balance += earnings * toUsers;

                if (Context.ContractsTillNextEvent <= 0)
                {
                    Context.ContractsTillNextEvent = Common.ContractsTillNextEvent;
                    return GameState.Event;
                }

                Context.ContractsTillNextEvent -= 1;
                
                // New contract
                return GameState.ChooseContractStart;
            }
            
            return null;
        }
        
        public override void StateEnded()
        {
            UIManager.Instance.HidePopup();
            Context.Company.ActiveContract = null;
            
            _votingState = null;
            _reward = 0;
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