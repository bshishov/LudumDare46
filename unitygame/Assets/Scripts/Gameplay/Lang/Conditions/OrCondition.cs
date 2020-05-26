using System.Linq;
using SimpleJSON;

namespace Gameplay.Lang.Conditions
{
    public class OrCondition : AggregatingCondition, ICondition
    {
        public OrCondition(JSONNode args) : base(args) {}
        
        public bool Call(GameContext context)
        {
            return Conditions.Any(c => c.Call(context));
        }
    }
}