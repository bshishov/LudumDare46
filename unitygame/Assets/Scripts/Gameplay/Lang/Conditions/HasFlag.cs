using System;
using SimpleJSON;
using Utils;

namespace Gameplay.Lang.Conditions
{
    public class HasFlagCondition : ICondition
    {
        private readonly string _flag;

        public HasFlagCondition(JSONObject args)
        {
            _flag = args.RequireString("flag");
        }
        
        public bool Call(GameContext context)
        {
            return context.Company.Flags.Contains(_flag);
        }
    }
}