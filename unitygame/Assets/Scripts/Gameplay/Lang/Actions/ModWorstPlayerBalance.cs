using System;
using System.Linq;
using Gameplay.Lang;
using SimpleJSON;
using UnityEngine;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class ModWorstPlayerBalance : IAction
    {
        private readonly float _amount;
        
        public ModWorstPlayerBalance(JSONObject args)
        {
            _amount = args.RequireFloat("amount");
        }
        
        public void Call(GameContext context)
        {
            var worst = context.Db.GetRichest(context.Db.GetActiveUsers().Count()).Last();
            worst.Balance = Mathf.Max(0, worst.Balance + _amount);
        }
    }
}