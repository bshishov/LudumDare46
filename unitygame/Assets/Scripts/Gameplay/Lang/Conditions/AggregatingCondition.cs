using System.Collections.Generic;
using SimpleJSON;

namespace Gameplay.Lang.Conditions
{
    public class AggregatingCondition
    {
        protected readonly List<ICondition> Conditions = new List<ICondition>();
        
        public AggregatingCondition(JSONNode args)
        {
            var conditionsRaw = args["conditions"];
            foreach (var conditionArgs in conditionsRaw.AsArray)
            {
                Conditions.Add(Registry.MakeCondition(conditionArgs.Value.AsObject));
            }
        }
    }
}