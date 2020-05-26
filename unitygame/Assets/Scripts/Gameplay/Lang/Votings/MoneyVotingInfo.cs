using SimpleJSON;
using Utils;

namespace Gameplay.Lang.Votings
{
    public class MoneyVotingInfo : IEventVoting
    {
        public readonly float StartAmount;
        public readonly float TargetAmount;
        public float Duration { get; private set; }
        public readonly bool ReturnMoneyAfterFailVoting;
        public readonly bool AllowMultipleVotes;
        public readonly IAction SuccessAction;
        public readonly IAction FailAction;
        
        public MoneyVotingInfo(JSONObject args)
        {
            Duration = args.OptionalFloat("duration").GetValueOrDefault(Common.DefaultVotingDuration);
            AllowMultipleVotes = args.OptionalBool("allow_multiple_votes").GetValueOrDefault(true);
            
            TargetAmount = args.RequireFloat("target");
            StartAmount = args.OptionalFloat("start").GetValueOrDefault(0);
            ReturnMoneyAfterFailVoting = args.OptionalBool("return_when_fail").GetValueOrDefault(false);
            
            SuccessAction = Registry.MakeAction(args.OptionalObject("success_action"));
            FailAction = Registry.MakeAction(args.OptionalObject("fail_action"));
        }

        public MoneyVotingInfo(
            float targetAmount,
            float startAmount = 0,
            float duration = Common.BankruptcyDuration,
            bool returnMoneyAfterFailVoting = false,
            bool allowMultipleVotes = true,
            IAction successAction = null,
            IAction failAction = null)            
        {
            Duration = duration;
            TargetAmount = targetAmount;
            StartAmount = startAmount;
            ReturnMoneyAfterFailVoting = returnMoneyAfterFailVoting;
            AllowMultipleVotes = allowMultipleVotes;
            SuccessAction = successAction;
            FailAction = failAction;
        }
    }
}