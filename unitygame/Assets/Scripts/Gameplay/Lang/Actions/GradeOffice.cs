using SimpleJSON;
using System;
using UnityEngine;
using Utils;

namespace Gameplay.Lang.Actions
{
    public class GradeOffice : IAction
    {
        private readonly int _grade;

        public GradeOffice(JSONObject args)
        {
            _grade = args.RequireInt("grade");            
        }
       
        public void Call(GameContext context)
        {
            context.Company.Level += _grade;
            context.Company.Expenses = Common.BaseSalaryExpenses[context.Company.Level - Common.BaseOfficeLevel];
        }
    }
}