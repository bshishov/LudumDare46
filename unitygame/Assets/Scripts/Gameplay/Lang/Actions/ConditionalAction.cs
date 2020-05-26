using SimpleJSON;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class ConditionalAction : IAction
    {
        private readonly ICondition _condition;
        private readonly IAction _action;
        private readonly IAction _elseAction;
        
        public ConditionalAction(JSONObject args)
        {
            _condition = Registry.MakeCondition(args.RequireObject("condition"));
            _action = Registry.MakeAction(args.RequireObject("action"));
            _elseAction = Registry.MakeAction(args.OptionalObject("else_action"));
        }
        
        public void Call(GameContext context)
        {
            var conditionResult = _condition.Call(context);
            if (conditionResult)
            {
                _action.Call(context);
            }
            else
            {
                _elseAction?.Call(context);
            }
        }
    }
}