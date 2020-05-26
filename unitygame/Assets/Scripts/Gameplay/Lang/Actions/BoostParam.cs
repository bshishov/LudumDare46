using SimpleJSON;
using System;
using UnityEngine;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class BoostParam : IAction
    {
        private readonly string _param;
        private readonly float _amount;

        public BoostParam(JSONObject args)
        {
            _param = args.RequireString("param");
            _amount = args.RequireFloat("amount");
        }
       
        public void Call(GameContext context)
        {
            if (context.Company.Params.ContainsKey(_param))
                context.Company.Params[_param] += _amount;
            else
                Debug.Log($"No {_param} in dictionary");
        }
    }
}