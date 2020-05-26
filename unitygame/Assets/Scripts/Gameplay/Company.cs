using System.Collections.Generic;
using Gameplay.Lang;
using Messages;
using UI;
using UnityEngine;
using Utils;
using Utils.Debugger;

namespace Gameplay
{
    public class Company: IUserCommandHandler, IUserMessageHandler
    {
        public string Name;

        public User Ceo
        {
            get => _ceo;
            set
            {
                _ceo = value;
                EventBus.Publish(new CompanyCeoChanged(this, value));
            }
        }

        public int Level
        {
            get => _officeLevel;
            set
            {
                _officeLevel = value;
                EventBus.Publish(new CompanyLevelChanged(this, value));
            }
        }

        public float Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                EventBus.Publish(new CompanyBalanceChanged(this, value));
            }
        }
        
        public float Specials;
        public float Expenses = Common.BaseSalaryExpenses[0];
        public readonly TimerState ExpensesTimer;


        public readonly Dictionary<string, float> Params = new Dictionary<string, float>()
        {
            {"ai", 0},
            {"art", 0},
            {"back", 0},
            {"rel", 0},
            {"front", 0},
            {"game", 0},
            {"inno", 0},
            {"mark", 0},
            {"tech", 0},
            {"ui", 0}
        };

        public ContractState ActiveContract
        {
            get => _activeContract;
            set
            {
                _activeContract = value;
                EventBus.Publish(new ActiveContractChanged(this, value));
                if(_activeContract != null)
                    Debug.Log($"[Company] New contract: {_activeContract.Info.Name}");
            }
        } 
        public bool HasContract => ActiveContract != null;

        public readonly HashSet<string> Flags = new HashSet<string>();

        private ContractState _activeContract;
        private int _officeLevel;
        private float _balance;
        private User _ceo;
            
        public Company(string name)
        {
            Name = name;
            Balance = Common.InitialCompanyBalance;
            Level = Common.BaseOfficeLevel;
            ExpensesTimer = new TimerState(Common.TimeToSalary);
            Debug.Log($"Created company with name {name}");
        }
        
        public void OnUserCommand(User user, string command, List<string> args)
        {
            ActiveContract?.OnUserCommand(user, command, args);
        }

        public void OnUserMessage(User user, string message)
        {
            ActiveContract?.OnUserMessage(user, message);
        }
    }
}