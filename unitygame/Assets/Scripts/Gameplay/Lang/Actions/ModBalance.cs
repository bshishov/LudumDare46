using SimpleJSON;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class ModBalance : IAction
    {
        private readonly float _amount;
        
        public ModBalance(JSONObject args)
        {
            _amount = args.RequireFloat("amount");
        }
        
        public void Call(GameContext context)
        {
            if(context.Company == null)
                return;
            
            context.Company.Balance += _amount;
        }
    }
}