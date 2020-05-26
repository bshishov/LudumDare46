using System.Collections.Generic;
using Twitch;
using Utils.FSM;

namespace Gameplay.States
{
    public abstract class BaseState : 
        IStateBehaviour<GameState>, 
        IUserCommandHandler, 
        IUserMessageHandler
    {
        protected readonly GameContext Context;
        protected readonly Client Client;
        protected readonly UserDb Db;
        
        protected BaseState(GameContext context, Client client, UserDb db)
        {
            Client = client;
            Context = context;
            Db = db;
        }
        
        public virtual void StateStarted() {}

        public abstract GameState? StateUpdate();

        public virtual void StateEnded() {}

        public virtual void OnUserCommand(User user, string command, List<string> args) {}
        public virtual void OnUserMessage(User user, string message) {}
    }
}