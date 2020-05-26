namespace Gameplay
{
    public interface IUserMessageHandler
    {
        void OnUserMessage(User user, string message);
    }
}