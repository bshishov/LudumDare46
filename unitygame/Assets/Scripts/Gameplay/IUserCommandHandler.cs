using System.Collections.Generic;

namespace Gameplay
{
    public interface IUserCommandHandler
    {
        void OnUserCommand(User user, string command, List<string> args);
    }
}