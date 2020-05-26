using System.Collections.Generic;

#if DEBUGGER
using Utils.Debugger;    
#endif

namespace Gameplay.Voting
{
    public class NotificationVotingState : IVotingState
    {
        public readonly TimerState Timer;
        
        public NotificationVotingState(float duration)
        {
            Timer = new TimerState(duration);
#if DEBUGGER
            Debugger.Default.Display("Skip Voting", Timer.Skip);
#endif
        }

        public void OnUserCommand(User user, string command, List<string> args)
        {
            if (command == "skip" && user.CanCheat)
                Timer.Skip();
        }

        public void OnUserMessage(User user, string message)
        {
        }

        public void Finish(GameContext context)
        {
        }

        public bool CanBeFinished()
        {
            return !Timer.IsActive;
        }

        public void Update()
        {
            Timer.Update();
        }
    }
}