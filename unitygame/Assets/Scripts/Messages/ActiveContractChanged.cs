using Gameplay;
using Utils;

namespace Messages
{
    public class ActiveContractChanged : GenericTinyMessage<ContractState>
    {
        public ActiveContractChanged(object sender, ContractState content) : base(sender, content)
        {
        }
    }
}