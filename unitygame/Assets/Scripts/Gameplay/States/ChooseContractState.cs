using Gameplay.Lang;
using Gameplay.Voting;
using Twitch;
using UI;
using Gameplay.Lang.Votings;
using Gameplay.Lang.Actions;
using System.Collections.Generic;

#if DEBUGGER
using UnityEngine;
using Utils.Debugger;
#endif

namespace Gameplay.States
{
    public class ChooseContractState : BaseState
    {
        private OptionsVotingState _activeVoting;

        public ChooseContractState(GameContext context, Client client, UserDb db) : base(context, client, db) { }

        public override void StateStarted()
        {
            /*
                при условии что контракта активного нет
                
                выбрать рандомный контракт
                открыть popup с названием описанием и reward?      
                
                делаем его активным      
            */

            var selectedContracts = ContractGenerator.ReturnContracts(Context, Db);
            var contractOptions = new List<OptionsVotingInfo.Option>();
            foreach (var contract in selectedContracts)
            {
                var option = new OptionsVotingInfo.Option
                {
                    Name = contract.Name, 
                    Action = new SetContract(contract),
                    IsEnabled = contract.Level <= Context.Company.Level
                };
                contractOptions.Add(option);
            }

            var optionsVotingInfo = new OptionsVotingInfo(true, contractOptions, Common.DefaultVotingDuration);
            _activeVoting = new OptionsVotingState(optionsVotingInfo);
            
            UIManager.Instance.ShowOptionsVotingPopup(new EventInfo("Choose contract", "Every next contract is more complicated than the previous one! But the harder the contract, the more money you will make..."), _activeVoting);
            SoundManager.Instance.Play(SoundBank.Instance.ContractStarted);
        }

        public override GameState? StateUpdate()
        {
            _activeVoting.Update();
            if (_activeVoting.CanBeFinished())
            {
                _activeVoting.Finish(Context);
                return GameState.Idle;
            }

            return null;
        }

        public override void StateEnded()
        {
            UIManager.Instance.HidePopup();

            var company = Context.Company;
            var contract = company.ActiveContract;

            if (contract != null)
            {
                foreach (var key in contract.WorkVotingState.Progress.Keys)
                {
                    if (company.Params.TryGetValue(key, out var value))
                        contract.WorkVotingState.Progress[key].CurrentAmount += value;
                }
            }
        }

        public override void OnUserMessage(User user, string message)
        {
            _activeVoting?.OnUserMessage(user, message);
        }

        public override void OnUserCommand(User user, string command, List<string> args)
        {
            _activeVoting?.OnUserCommand(user, command, args);
        }
    }
}