namespace Gameplay.Voting
{
    public interface IVotingState : IUserCommandHandler, IUserMessageHandler
    {
        void Finish(GameContext context);
        bool CanBeFinished();
        void Update();
    }
}