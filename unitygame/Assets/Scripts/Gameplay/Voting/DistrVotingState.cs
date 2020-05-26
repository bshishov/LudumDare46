using System.Collections.Generic;
using System.Linq;
using Gameplay.Lang.Votings;
using UnityEngine;
using Utils;
using System;

#if DEBUGGER
using Utils.Debugger;
#endif

namespace Gameplay.Voting
{
    public class DistrVotingState : IVotingState
    {
        public float TimeRemaining => Timer.TimeRemaining;
        
        public readonly OptionsVotingInfo Info;
        public bool IsActive;
        public readonly TimerState Timer;
        public readonly HashSet<User> Voters = new HashSet<User>();
        public readonly Dictionary<string, OptionsVotingInfo.Option> Options = new Dictionary<string, OptionsVotingInfo.Option>();
        public readonly Dictionary<string, float> Results = new Dictionary<string, float>();
        
        public DistrVotingState(OptionsVotingInfo info)
        {
            Info = info;
            Timer = new TimerState(info.Duration);
            IsActive = true;

            foreach (var option in info.Options)
            {
                Options.Add(option.Name, option);
                Results.Add(option.Name, 50);
            }
#if DEBUGGER
            Debugger.Default.Display("Skip Voting", Timer.Skip);
#endif
        }

        public bool Vote(User user, string option)
        {
            if (!IsActive)
                return false;

            if (!Options.ContainsKey(option))
                return false;
            
            if (!Info.AllowMultipleVotes && Voters.Contains(user))
                return false;

            Debug.Log($"Received vote for {option} from {user.Name}");
            SoundManager.Instance.Play(SoundBank.Instance.VoteReceived);

            Results[option] += Math.Min(Common.DistrStep, 100 - Results[option]);
            foreach (var tmpOption in Results.Keys.ToList())    
                if (tmpOption != option)
                    Results[tmpOption] -= Math.Min(Common.DistrStep, Results[tmpOption]);
            
            Voters.Add(user);
            return true;
        }

        public void Finish(GameContext context)
        {
            if (IsActive)
            {
                var option = HighestRatedOption();
                option?.Action?.Call(context);
                IsActive = false;
            }
        }

        public bool CanBeFinished()
        {
            return !Timer.IsActive;
        }

        public void Update()
        {
            if(IsActive)
                Timer.Update();
        }

        public OptionsVotingInfo.Option HighestRatedOption()
        {
            var optionName = Results.OrderByDescending(r => r.Value).First().Key;
            return Options[optionName];
        }

        public void OnUserCommand(User user, string command, List<string> args)
        {
            // Todo handle votes
            if (command == "vote" && args.Parse(out int number))
            {
                if (number >= 1 && number <= Options.Count)
                {
                    var option = Results.Keys.ToList()[number - 1];
                    Vote(user, option);
                }
            }
            
            if (command == "skip" && user.CanCheat)
                Timer.Skip();
        }

        public void OnUserMessage(User user, string message)
        {
            if (int.TryParse(message, out var number))
            {
                if (number >= 1 && number <= Options.Count)
                {
                    var option = Results.Keys.ToList()[number - 1];
                    Vote(user, option);
                }
            }

            if (Options.ContainsKey(message))
                Vote(user, message);
        }
    }
}