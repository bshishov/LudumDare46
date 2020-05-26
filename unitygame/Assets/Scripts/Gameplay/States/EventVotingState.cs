using System.Collections.Generic;
using Gameplay.Lang;
using Gameplay.Lang.Votings;
using Gameplay.Voting;
using Twitch;
using UI;
using UnityEngine;

namespace Gameplay.States
{
    public class EventVotingState : BaseState
    {
        private GameObject _activeVotingUI;
        
        private MoneyVotingState _moneyVotingState;
        private OptionsVotingState _optionsVotingState;
        private IVotingState _activeVoting;
        private EventInfo _event;
        
        public EventVotingState(GameContext context, Client client, UserDb db) : base(context, client, db) {}

        public override void StateStarted()
        {
            /*
             1. Выбираем рандомное доступное событие
                если оно есть
                    открываем popup с событием
                    ставим таймер на просмотр / голосование
            */

            _event = Context.GetRandomAvailableEvent();
            if (_event != null)
            {
                if (_event.Voting != null)
                {
                    // Если это голосование на деньги
                    if (_event.Voting is MoneyVotingInfo moneyVotingInfo)
                    {
                        _moneyVotingState = new MoneyVotingState(moneyVotingInfo);
                        _activeVoting = _moneyVotingState;
                        UIManager.Instance.ShowMoneyVotingPopup(_event, _moneyVotingState);
                    }

                    // Если это голосование с выбором ответа
                    if (_event.Voting is OptionsVotingInfo optionsVotingInfo)
                    {
                        _optionsVotingState = new OptionsVotingState(optionsVotingInfo);
                        _activeVoting = _optionsVotingState;
                        UIManager.Instance.ShowOptionsVotingPopup(_event, _optionsVotingState);
                    }
                }
                else
                {
                    _activeVoting = new NotificationVotingState(Common.InformationPopupDisplayTime);
                    UIManager.Instance.ShowNotificationPopup(_event);
                }
                
                SoundManager.Instance.Play(SoundBank.Instance.NewEvent);
            }
        }

        public override GameState? StateUpdate()
        {
            /*
             0. ** обновляем UI голосования **
             1. Считаем таймер на показ / уведомление
                если таймер закончился
                    если голосование MoneyVoting
                        если success
                            выполняем SuccessAction
                        если fail
                            выполняем FailAction
                    если голосование OptionsVoting
                        выбираем вариант с наибольшим количеством голосов
                        выболняем Action этого варианта
                        
                    выполняем event.Action
                    переходим в Idle
             */

            // No event (for some reason)
            if (_event == null || _activeVoting == null)
                return GameState.Idle;


            _activeVoting.Update();
            if (_activeVoting.CanBeFinished())
            {
                _activeVoting.Finish(Context);
                _event.Action?.Call(Context);
                return GameState.Idle;
            }
            
            return null;
        }

        public override void StateEnded()
        {
            // скрываем popup голосования / уведомления
            UIManager.Instance.HidePopup();
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