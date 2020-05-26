using SimpleJSON;
using UnityEngine;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class ModSalary : IAction
    {
        private readonly float _amount;
        
        public ModSalary(JSONObject args)
        {
            _amount = args.RequireFloat("amount");
        }
        
        public void Call(GameContext context)
        {
            if(context.Company != null)
                context.Company.Expenses = Mathf.Max(0, context.Company.Expenses + _amount);
        }
    }
}