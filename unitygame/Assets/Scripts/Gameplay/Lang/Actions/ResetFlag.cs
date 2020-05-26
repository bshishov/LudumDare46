using SimpleJSON;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class ResetFlag: IAction
    {
        private readonly string _flag;

        public ResetFlag(JSONObject args)
        {
            _flag = args.RequireString("flag");
        }
        
        public void Call(GameContext context)
        {
            if(context.Company.Flags.Contains(_flag))
                context.Company.Flags.Remove(_flag);
        }
    }
}