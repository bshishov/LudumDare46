using Gameplay;
using Utils;

namespace Messages
{
    public class CompanyCeoChanged : GenericTinyMessage<User>
    {
        public CompanyCeoChanged(object sender, User content) : base(sender, content)
        {
        }
    }
}