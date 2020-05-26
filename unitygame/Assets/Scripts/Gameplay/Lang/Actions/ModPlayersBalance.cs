using SimpleJSON;
using UnityEngine;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class ModPlayersBalance : IAction
    {
        private readonly float _amount;
        
        public ModPlayersBalance(JSONObject args)
        {
            _amount = args.RequireFloat("amount");
        }
        
        public ModPlayersBalance(float amount)
        {
            _amount = amount;
        }
        
        public void Call(GameContext context)
        {
            foreach (var user in context.Db.GetActiveUsers())
            {
                user.Balance = Mathf.Max(0, user.Balance + _amount);
            }
        }
    }
}