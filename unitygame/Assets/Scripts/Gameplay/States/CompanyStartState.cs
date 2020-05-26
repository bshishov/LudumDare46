using System.Collections.Generic;
using System.Linq;
using Gameplay.Lang;
using Gameplay.Lang.Votings;
using Gameplay.Voting;
using Twitch;
using UI;
using Utils;

namespace Gameplay.States
{
    public class CompanyStartState : BaseState
    {
        private readonly List<string> _names;
        private OptionsVotingState _votingState;
        private EventInfo _event;

        public CompanyStartState(GameContext context, Client client, UserDb db) : base(context, client, db)
        {
            _names = Registry.LoadListOfStrings(Common.CompanyNamesPath);
        }
        
        public override void StateStarted()
        {
            // Выводим окошко с OptionsVoting за название (выбираем рандомные 4), ставим таймер
            _event = new EventInfo(
                "Choose new name for company!", 
                "Looks like the previous company wasn't as successful as we wanted it to be. But the new mysterious investor wants to try it all again! Even better, he wants to keep the previous board.");

            var randomNames = RandomUtils.NRandom(_names, 4);
            var votingOptions = randomNames.Select(name => new OptionsVotingInfo.Option {Name = name});
            var votingInfo = new OptionsVotingInfo(allowMultipleVotes: true, votingOptions, Common.DefaultVotingDuration);
            _votingState = new OptionsVotingState(votingInfo);
            UIManager.Instance.ShowOptionsVotingPopup(_event, _votingState);
            SoundManager.Instance.Play(SoundBank.Instance.NewCompany);
        }
        
        public override GameState? StateUpdate()
        {
            /*
             0. Обновляем UI голосования
             1. Проверяем таймер голосования
                если таймер законичлся
                    создаем новую компанию с названием - победителем
                    переходим в Idle                
             */
            if (_votingState == null)
                return GameState.Idle;
            
            _votingState.Update();
            if (_votingState.CanBeFinished())
            {
                _votingState.Finish(Context);
                Context.Company = new Company(_votingState.HighestRatedOption().Name);
                return GameState.Idle;
            }
            return null;
        }

        public override void StateEnded()
        {
            // скрываем окно голосования
            UIManager.Instance.HidePopup();
        }

        public override void OnUserCommand(User user, string command, List<string> args)
        {
            _votingState?.OnUserCommand(user, command, args);
        }

        public override void OnUserMessage(User user, string message)
        {
            _votingState?.OnUserMessage(user, message);
        }
    }
}