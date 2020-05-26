using System.Collections.Generic;
using System.Linq;
using Gameplay.Lang;
using Gameplay.Lang.Votings;
using Twitch;
using UnityEngine;
using Utils;

namespace Gameplay
{
    public static class ContractGenerator
    {
        public static IEnumerable<ContractInfo> ReturnContracts(GameContext context, UserDb db)
        {
            var availableContracts = context.Contracts.Where(c => c.IsAvailable(context));
            
            var minPower = db.CountActive() * Common.InintialWorkPower;
            var contracts = new List<ContractInfo>();            
            var contractNames = new HashSet<string>();

            for (var level = 1; level < 5; level++)
            {
                /*
                var mult = Mathf.Pow(2, (_contract.i - 1));
                var difficulty = Mathf.Max(mult * minPower, 1.4f * Common.InintialWorkPower);
                contract.Earnings = Mathf.Pow(_contract.value.Reward, 0.1f*_contract.i + 0.9f)*(0.9f + 0.2f * Random.value);

                if (context.Company.Level > _contract.i)                
                    contract.DisplayName = $"{contract.Info.Name} -available, R: {contract.Earnings: 0}$, D: {contract.Difficulty}";
                else
                    contract.DisplayName += $"{contract.Info.Name} - office too small,  R: {contract.Earnings: 0}$";
                */
                var mult = Mathf.Pow(2, (level - 3));
                var difficulty = Mathf.Max(mult * minPower, Mathf.Pow(level, 2) * Common.InintialWorkPower);  

                var contractsLvlBased = availableContracts.Where(c => c.Level == level && !contractNames.Contains(c.Name));
                var baseContract = RandomUtils.Choice(contractsLvlBased.ToList());
                contractNames.Add(baseContract.Name);

                var baseVoting = baseContract.WorkVoting;
                var options = baseVoting.Options.Select(old => new WorkVotingInfo.Option()
                {
                    TargetAmount = Mathf.Floor(difficulty * old.TargetAmount),
                    Name = $"{old.Name}\n <color=#AAA>!{old.Key}</color>",
                    Key = old.Key,
                    CanPay = old.CanPay,
                    CanWork = old.CanWork,
                    IsEnabled = old.IsEnabled
                });
                
                var voting = new WorkVotingInfo(
                    options, 
                    baseVoting.Duration, 
                    baseVoting.AllowMultipleVotes, 
                    baseVoting.SuccessAction, 
                    baseVoting.FailAction);

                var reward = Mathf.Pow(baseContract.Reward, 0.1f * level + 0.8f) * (0.9f + 0.2f * Random.value); // TODO: balance
                var name = $"{baseContract.Name} \nlvl:{level} reward: ${reward:N0}"; // TODO: balance
                var description = baseContract.Description;
                
                var contract = new ContractInfo(
                    name, 
                    description, 
                    voting, 
                    level, 
                    reward);

                contracts.Add(contract);
            }
            return contracts;
        }
    }
}