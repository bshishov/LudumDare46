using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using Utils;

namespace Gameplay.Lang.Votings
{
    public class WorkVotingInfo : IEventVoting
    {
        public class Option
        {
            public string Name;
            public string Key;
            public float TargetAmount = 100;
            public float Initial = 0f;
            public bool CanWork = true;
            public bool CanPay = true;
            public bool IsEnabled = true;
        }

        public float Duration { get; private set; }
        
        public readonly bool AllowMultipleVotes;
        
        public readonly IAction SuccessAction;
        public readonly IAction FailAction;

        public readonly List<Option> Options;

        public WorkVotingInfo(JSONObject json)
        {
            Duration = json.OptionalFloat("duration").GetValueOrDefault(Common.DefaultContractDuration);
            AllowMultipleVotes = json.OptionalBool("allow_multiple_votes").GetValueOrDefault(false);

            
            Options = new List<Option>();
            foreach (var optionArgs in json.RequireArray("options"))
            {
                var optionObj = optionArgs.Value.AsObject;
                Options.Add(new Option 
                {
                    Name = optionObj.RequireString("name"),
                    Key = optionObj.RequireString("key"),
                    CanPay = optionObj.OptionalBool("can_pay").GetValueOrDefault(true),
                    CanWork = optionObj.OptionalBool("can_work").GetValueOrDefault(true),
                    Initial = optionObj.OptionalFloat("initial").GetValueOrDefault(0f),
                    TargetAmount = optionObj.RequireFloat("target"),
                    IsEnabled = optionObj.OptionalBool("enabled").GetValueOrDefault(true)
                });
            }
        }

        public WorkVotingInfo(
            IEnumerable<Option> options,
            float duration = Common.DefaultContractDuration,
            bool allowMultipleVotes = false,
            IAction successAction = null,
            IAction failAction = null)            
        {
            Duration = duration;
            Options = options.ToList();
            AllowMultipleVotes = allowMultipleVotes;
            SuccessAction = successAction;
            FailAction = failAction;
        }

        public static WorkVotingInfo Example()
        {
            // TODO: Delete this when all contracts json is ready
            var options = new[]
            {
                new WorkVotingInfo.Option
                {
                    Name = "Backend Programming",
                    Key = "backend",
                    TargetAmount = 400
                },
                new WorkVotingInfo.Option
                {
                    Name = "Frontend Programming",
                    Key = "frontend",
                    TargetAmount = 500
                },
                new WorkVotingInfo.Option
                {
                    Name = "THIS",
                    Key = "marketing",
                    TargetAmount = 300
                },
                new WorkVotingInfo.Option
                {
                    Name = "IS",
                    Key = "marketing1",
                    TargetAmount = 300
                },
                new WorkVotingInfo.Option
                {
                    Name = "EXAMPLE",
                    Key = "marketing2",
                    TargetAmount = 300
                }
            };
                
            return new WorkVotingInfo(options);
        }
    }
}