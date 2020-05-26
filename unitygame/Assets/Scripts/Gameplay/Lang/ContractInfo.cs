using System.Linq;
using Gameplay.Lang.Votings;
using SimpleJSON;
using Utils;

namespace Gameplay.Lang
{
    public class ContractInfo
    {
        public readonly int Level;
        public readonly string Name;
        public readonly string Description;
        public readonly float Reward;
        public readonly WorkVotingInfo WorkVoting;
        public readonly ICondition Condition;
        public readonly IAction SuccessAction;
        public readonly IAction FailAction;

        public float TotalDifficulty => WorkVoting.Options.Sum(o => o.TargetAmount);
        
        public ContractInfo(JSONObject args)
        {
            Name = args.RequireString("name");
            Description = args.RequireString("description");
            Level = args.OptionalInt("level").GetValueOrDefault(Common.DefaultContractLevel);

            var workVotingArgs = args.OptionalObject("work");
            if (workVotingArgs != null)
            {
                WorkVoting = new WorkVotingInfo(workVotingArgs);
            }
            else
            {
                // TODO: DELETE AFTER ALL JSON IS COMPLETED
                WorkVoting = WorkVotingInfo.Example();
            }

            Reward = args.OptionalFloat("reward").GetValueOrDefault(Common.DefaultContractReward);

            SuccessAction = Registry.MakeAction(args.OptionalObject("success_action"));
            FailAction = Registry.MakeAction(args.OptionalObject("fail_action"));
            Condition = Registry.MakeCondition(args.OptionalObject("condition"));
        }

        public ContractInfo(
            string name,
            string description,
            WorkVotingInfo workVoting,
            int level = Common.DefaultContractLevel,
            float reward = Common.DefaultContractReward,
            IAction successAction = null,
            IAction failAction = null,
            ICondition condition = null
        )
        {
            Name = name;
            Description = description;
            Level = level;
            WorkVoting = workVoting;
            Reward = reward;
            SuccessAction = successAction;
            FailAction = failAction;
            Condition = condition;
        }

        public bool IsAvailable(GameContext context)
        {
            if (Condition != null)
                return Condition.Call(context);

            return true;
        }
    }
}