using UnityEngine;

namespace Gameplay.Lang.Actions
{
    public class SetContract : IAction
    {
        private readonly ContractInfo _contract;

        public SetContract(ContractInfo contract)
        {
            _contract = contract;
        }
       
        public void Call(GameContext context)
        {   
            context.Company.ActiveContract = new ContractState(_contract);
        }
    }
}