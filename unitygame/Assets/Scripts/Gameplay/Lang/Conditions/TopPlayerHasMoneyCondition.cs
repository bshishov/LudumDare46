using System;
using SimpleJSON;
using Utils;
using System.Linq;


namespace Gameplay.Lang.Conditions
{
    public class TopPlayerHasMoneyCondition : ICondition
    {
        private readonly float _amount;

        public TopPlayerHasMoneyCondition(JSONObject args)
        {
            _amount = args.RequireFloat("amount");           
        }
        
        public bool Call(GameContext context)
        {
            var richest = context.Db.GetRichest(1).First();
            return richest.Balance > _amount;
        }
    }
}