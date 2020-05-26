using SimpleJSON;

namespace Gameplay.Lang.Conditions
{
    public class NotCondition : ICondition
    {
        private readonly ICondition _condition;

        public NotCondition(JSONNode node)
        {
            _condition = Registry.MakeCondition(node["condition"].AsObject);
        }
        
        public bool Call(GameContext context)
        {
            return !_condition.Call(context);
        }
    }
}