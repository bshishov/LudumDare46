using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using Utils;

namespace Gameplay.Lang.Votings
{
    public class OptionsVotingInfo : IEventVoting
    {
        public class Option
        {
            public string Name;
            public IAction Action;
            public bool IsEnabled = true;
        }
        
        public float Duration { get; private set; }
        public readonly bool AllowMultipleVotes;
        public readonly List<Option> Options = new List<Option>();
        
        public OptionsVotingInfo(JSONObject args)
        {
            Duration = args.OptionalFloat("duration").GetValueOrDefault(Common.DefaultVotingDuration);
            AllowMultipleVotes = args.OptionalBool("allow_multiple_votes").GetValueOrDefault(true);

            foreach (var optionArgs in args.RequireArray("options"))
            {
                var optionObj = optionArgs.Value.AsObject;
                Options.Add(new Option
                {
                    Name = optionObj.RequireString("name"),
                    Action = Registry.MakeAction(optionObj.OptionalObject("action"))
                });
            }
        }

        public OptionsVotingInfo(bool allowMultipleVotes, IEnumerable<Option> options, float duration)
        {
            AllowMultipleVotes = allowMultipleVotes;
            Options = options.ToList();
            Duration = duration;
        }
    }
}