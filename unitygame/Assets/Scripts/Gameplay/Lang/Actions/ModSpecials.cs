using SimpleJSON;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class ModSpecials : IAction
    {
        private readonly float _amount;
        
        public ModSpecials(JSONObject args)
        {
            _amount = args.RequireFloat("amount");
        }
        
        public void Call(GameContext context)
        {
            if(context.Company != null)
                context.Company.Specials += _amount;
        }
    }
}