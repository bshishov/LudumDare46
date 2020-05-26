using SimpleJSON;
using Utils;

namespace Gameplay.Lang
{
    public class EventInfo
    {
        public readonly string Title;
        public readonly string Description;
        public readonly ICondition Condition;
        public readonly IEventVoting Voting;
        public readonly IAction Action;

        public EventInfo(JSONObject args)
        {
            Title = args.RequireString("title");
            Description = args.RequireString("description");
            Condition = Registry.MakeCondition(args.OptionalObject("condition"));
            Voting = Registry.MakeVoting(args.OptionalObject("voting"));
            Action = Registry.MakeAction(args.OptionalObject("action"));
        }

        public EventInfo(
            string title, 
            string description, 
            ICondition condition = null, 
            IAction action = null,
            IEventVoting voting = null)
        {
            Title = title;
            Description = description;
            Condition = condition;
            Action = action;
            Voting = voting;
        }

        public bool IsAvailable(GameContext context)
        {
            if (Condition == null)
                return true;

            return Condition.Call(context);
        }
    }
}