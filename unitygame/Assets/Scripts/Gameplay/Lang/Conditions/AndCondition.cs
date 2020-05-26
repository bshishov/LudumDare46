using System.Linq;
using SimpleJSON;

namespace Gameplay.Lang.Conditions
{
    public class AndCondition : AggregatingCondition, ICondition
    {
        public AndCondition(JSONNode args) : base(args) {}
        
        public bool Call(GameContext context)
        {
            return Conditions.All(c => c.Call(context));
        }
    }
}