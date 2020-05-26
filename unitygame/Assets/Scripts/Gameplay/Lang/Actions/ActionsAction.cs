using System.Collections.Generic;
using SimpleJSON;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class ActionsAction : IAction
    {
        private readonly List<IAction> _actions = new List<IAction>();
        
        public ActionsAction(JSONObject args)
        {
            foreach (var objArgs in args.RequireArray("actions"))
            {
                _actions.Add(Registry.MakeAction(objArgs.Value.AsObject));   
            }
        }
        
        public void Call(GameContext context)
        {
            foreach (var action in _actions)
                action.Call(context);
        }
    }
}