using SimpleJSON;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class SendMessageAction : IAction
    {
        private readonly string _message;
        
        public SendMessageAction(JSONObject args)
        {
            _message = args.RequireString("message");
        }
        
        public void Call(GameContext context)
        {
            context.Client.SendMessage(_message);
        }
    }
}