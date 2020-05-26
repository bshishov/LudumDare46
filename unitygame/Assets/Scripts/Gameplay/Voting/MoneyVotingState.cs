using System.Collections.Generic;
using Gameplay.Lang.Votings;
using UnityEngine;
using Utils;

#if DEBUGGER
using Utils.Debugger;    
#endif


namespace Gameplay.Voting
{
    public class MoneyVotingState : IVotingState
    {
        public float TimeRemaining => Timer.TimeRemaining;
        
        public readonly MoneyVotingInfo Info;
        
        public bool IsActive = true;
        public float CurrentAmount;
        public readonly TimerState Timer;
        public readonly HashSet<User> Voters = new HashSet<User>();
        public readonly Dictionary<User, float> Votes = new Dictionary<User, float>();

        public float ClampedProgress => Mathf.Clamp01(CurrentAmount / Info.TargetAmount);

        public MoneyVotingState(MoneyVotingInfo info)
        {
            Info = info;
            CurrentAmount = Info.StartAmount;
            Timer = new TimerState(info.Duration);
#if DEBUGGER
            Debugger.Default.Display("Skip Voting", Timer.Skip); 
#endif
        }

        public bool Vote(User user, float amount)
        {
            if (!IsActive)
                return false;

            if (amount <= 0)
                return false;

            if (!Info.AllowMultipleVotes && Voters.Contains(user))
                return false;

            if (!user.CanSpend(amount))
                // Can't vote then
                return false;

            user.Spend(amount);
            CurrentAmount += amount;

            if (Votes.ContainsKey(user))
                Votes[user] += amount;
            else
            {
                Votes.Add(user, amount);
            }

            SoundManager.Instance.Play(SoundBank.Instance.VoteReceived);
            return true;
        }

        public bool CanBeFinished()
        {
            return !Timer.IsActive || CurrentAmount >= Info.TargetAmount;
        }

        public void Update()
        {
            if(IsActive)
                Timer.Update();
        }

        public void Finish(GameContext context)
        {
            if (IsActive)
            {

                if (CurrentAmount >= Info.TargetAmount)
                {
                    //Success
                    Info.SuccessAction?.Call(context);
                }
                else
                {
                    // Fail
                    Info.FailAction?.Call(context);

                    if (Info.ReturnMoneyAfterFailVoting)
                    {
                        foreach (var kvp in Votes)
                        {
                            var user = kvp.Key;
                            var invested = kvp.Value;

                            user.Balance += invested;
                        }
                    }
                }
                IsActive = false;
            }
        }

        public void OnUserCommand(User user, string command, List<string> args)
        {
            if (command == "donate" && args.Parse(out int amount))
            {
                Vote(user, amount);
            }

            if (command == "skip" && user.CanCheat)
                Timer.Skip();
        }

        public void OnUserMessage(User user, string message)
        {
            if (int.TryParse(message, out var amount))
                Vote(user, amount);
        }
    }
}