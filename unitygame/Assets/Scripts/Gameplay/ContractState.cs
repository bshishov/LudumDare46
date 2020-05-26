using System.Collections.Generic;
using Gameplay.Lang;
using Gameplay.Voting;
using UnityEngine;

namespace Gameplay
{
    public class ContractState: IUserCommandHandler, IUserMessageHandler
    {
        public readonly ContractInfo Info;
        public readonly WorkVotingState WorkVotingState;

        public ContractState(ContractInfo info)
        {
            Info = info;
            WorkVotingState = new WorkVotingState(info.WorkVoting);
        }

        public void OnUserCommand(User user, string command, List<string> args)
        {
            WorkVotingState.OnUserCommand(user, command, args);
        }

        public void OnUserMessage(User user, string message)
        {
            WorkVotingState.OnUserMessage(user, message);
        }
    }
}