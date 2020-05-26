using System;
using System.Linq;
using Gameplay.Lang;
using SimpleJSON;
using UnityEngine;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class ModRandomPlayerBalance : IAction
    {
        private readonly float _amount;
        
        public ModRandomPlayerBalance(JSONObject args)
        {
            _amount = args.RequireFloat("amount");
        }
        
        public void Call(GameContext context)
        {
            var randomActiveUser = RandomUtils.Choice(context.Db.GetActiveUsers().ToList());
            randomActiveUser.Balance = Mathf.Max(0, randomActiveUser.Balance + _amount);
        }
    }
}