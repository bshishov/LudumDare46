using SimpleJSON;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class SetFlag: IAction
    {
        private readonly string _flag;

        public SetFlag(JSONObject args)
        {
            _flag = args.RequireString("flag");
        }
        
        public void Call(GameContext context)
        {
            context.Company.Flags.Add(_flag);
        }
    }
}