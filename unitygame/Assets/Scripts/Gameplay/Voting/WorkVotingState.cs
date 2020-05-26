using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Lang;
using Gameplay.Lang.Votings;
using UnityEngine;
using Utils;

#if DEBUGGER
using Utils.Debugger;    
#endif


namespace Gameplay.Voting
{
    public class WorkVotingState : IVotingState
    {
        
        public class WorkProgress
        {
            public readonly WorkVotingInfo.Option Option;
            public float CurrentAmount;

            public float Progress => Mathf.Clamp01(CurrentAmount / Option.TargetAmount);
            public readonly Dictionary<User, float> Result = new Dictionary<User, float>();
            public bool IsCompleted => CurrentAmount >= Option.TargetAmount;
            
            public WorkProgress(WorkVotingInfo.Option option)
            {
                Option = option;
                CurrentAmount = option.Initial;
            }

            public void Add(User user, float amount)
            {
                if (Result.ContainsKey(user))
                    Result[user] += amount;
                else
                    Result.Add(user, amount);
                
                CurrentAmount += amount;
            }

            public bool HasVoted(User user)
            {
                return Result.ContainsKey(user);
            }
        }
        
        
        public float TimeRemaining => Timer.TimeRemaining;
        
        public readonly WorkVotingInfo Info;
        public bool IsActive { get; private set; } = true;
        public readonly TimerState Timer;
        public readonly Dictionary<string, WorkProgress> Progress = new Dictionary<string, WorkProgress>(StringComparer.InvariantCultureIgnoreCase);
        public readonly HashSet<User> Voters = new HashSet<User>();

        public WorkVotingState(WorkVotingInfo info)
        {
            Info = info;
            Timer = new TimerState(Info.Duration);

            foreach (var option in Info.Options)
                Progress.Add(option.Key, new WorkProgress(option));
            
#if DEBUGGER
            Debugger.Default.Display("Skip Voting", Timer.Skip);
#endif
        }

        public bool Work(User user, string key)
        {
            if (!IsActive)
            {
                Debug.Log($"Work voting is not active");
                return false;
            }

            if (!Progress.ContainsKey(key))
                // No such option
                return false;

            if (!Info.AllowMultipleVotes && Voters.Contains(user))
            {
                Debug.Log($"Work already voted {user.Name}");
                return false;
            }
            
            
            var progress = Progress[key];

            if (!progress.Option.CanWork)
                // Cant work for that option
                return false;
            
            progress.Add(user, user.WorkPower);
            Voters.Add(user);
            
            Debug.Log($"Work done by {user.Name} amount {user.WorkPower}");
            

            SoundManager.Instance.Play(SoundBank.Instance.VoteReceived);
            return true;
        }

        public bool Pay(User user, string key, float amount)
        {
            if (!IsActive)
            {
                Debug.Log($"Pay not active");
                return false;
            }

            if (!Progress.ContainsKey(key))
                // No such option
                return false;
                

            if (amount <= 0)
            {
                Debug.Log($"Pay amount <=");
                return false;
            }               

            if (!user.CanSpend(amount))
            {
                Debug.Log($"Pay can't spend");
                return false;
            }

            var progress = Progress[key];
            
            if (!progress.Option.CanPay)
                // Cant pay for that option
                return false;
            
            progress.Add(user, amount / Common.MoneyToWorkPointsRatio);
            user.Spend(amount);
            
            Debug.Log($"Pay done by {user.Name}");
            SoundManager.Instance.Play(SoundBank.Instance.VoteReceived);
            return true;
        }

        public bool CanBeFinished()
        {
            return !Timer.IsActive || IsSuccessful();
        }

        public bool IsSuccessful()
        {
            return Progress.Values.All(p => p.IsCompleted);
        }

        public void Update()
        {
            Timer.Update();
        }

        public void Finish(GameContext context)
        {
            if (IsActive)
            {
                if (IsSuccessful())
                {
                    foreach (var user in Voters)
                        user.WorkPower += Common.WorkPowerBoost;
                    
                    //Success
                    Info.SuccessAction?.Call(context);
                }
                else
                {
                    // Fail
                    Info.FailAction?.Call(context);
                }
                IsActive = false;
            }
        }

        public void OnUserCommand(User user, string command, List<string> args)
        {
            if (command == "pay")
            {
                if(args.Count != 2)
                    return;

                var arg1 = args[0];
                var arg2 = args[1];

                if (Progress.ContainsKey(arg1) && int.TryParse(arg2, out var amount2))
                    Pay(user, arg1, amount2);
                
                // Different arg combination
                if (Progress.ContainsKey(arg2) && int.TryParse(arg1, out var amount1))
                    Pay(user, arg2, amount1);
            }

            if (command == "work" && args.Parse(out int optionIndex))
            {
                // Work voting by option number !work 3 
                if (optionIndex >= 1 && optionIndex <= Progress.Count)
                    Work(user, Progress.Keys.ToList()[optionIndex - 1]);
            }

            if (command == "work" && args.Parse(out string optionKey))
            {
                // Work voting by option key !work backend
                Work(user, optionKey);
            }

            if (command == "skip" && user.CanCheat)
                Timer.Skip();
            
            if (Progress.ContainsKey(command))
                // User simply types "!backend"
                Work(user, command);
        }

        public void OnUserMessage(User user, string message)
        {
            if (Progress.ContainsKey(message))
                // User simply types "backend"
                Work(user, message);
        }
    }
}