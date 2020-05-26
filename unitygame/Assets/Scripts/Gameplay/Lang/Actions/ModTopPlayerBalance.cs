using System.Linq;
using SimpleJSON;
using UnityEngine;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class ModTopPlayerBalance : IAction
    {
        private readonly float _amount;
        
        public ModTopPlayerBalance(JSONObject args)
        {
            _amount = args.RequireFloat("amount");
        }
        
        public void Call(GameContext context)
        {
            var richest = context.Db.GetRichest(1).First();
            richest.Balance = Mathf.Max(0, richest.Balance + _amount);
        }
    }
}