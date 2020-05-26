using System;
using System.Linq;
using Gameplay.Lang;
using SimpleJSON;
using UnityEngine;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class ModSelectedPlayerBalance : IAction
    {
        private readonly float _amount;
        
        public ModSelectedPlayerBalance(JSONObject args)
        {
            _amount = args.RequireFloat("amount");
        }
        
        public void Call(GameContext context)
        {
            var user = context.Db.GetActiveUsers().FirstOrDefault(u => u.Name.Equals(context.SelectedOption));
            if (user != null)
            {
                user.Balance = Mathf.Max(0, user.Balance + _amount);
            }
        }
    }
}