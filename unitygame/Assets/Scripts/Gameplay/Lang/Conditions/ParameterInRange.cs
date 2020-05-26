using System;
using SimpleJSON;
using Utils;

namespace Gameplay.Lang.Conditions
{
    public class ParameterInRangeCondition : ICondition
    {
        private readonly string _parameterName;
        private readonly float _minVal;
        private readonly float _maxVal;

        public ParameterInRangeCondition(JSONObject args)
        {
            _parameterName = args.RequireString("param");
            _minVal = args.OptionalFloat("min").GetValueOrDefault(float.MinValue);
            _maxVal = args.OptionalFloat("max").GetValueOrDefault(float.MaxValue);
            
            if(!GameContext.AvailableParams.Contains(_parameterName))
                throw new ArgumentException($"No such parameter: \"{_parameterName}\" in {args}");
        }
        
        public bool Call(GameContext context)
        {
            var val = context.GetParameterValue(_parameterName);
            return val > _minVal && val < _maxVal;
        }
    }
}