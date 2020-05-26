using System;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class User
    {
        [NonSerialized] public bool IsActive = false;
        [NonSerialized] public float LastInteraction = 0;
        
        public string Name;
        public float Balance;
        
        public bool IsModerator;
        public bool IsBroadcaster;
        public bool IsSubscriber;
        
        public float SpentMoneyOnCompany;
        public float SpentMoneyOnSelf;

        public float WorkPower;

        public bool CanCheat => IsModerator || IsBroadcaster;

        public User(string name, float balance, float workPower)
        {
            Name = name;
            Balance = balance;
            WorkPower = workPower; 
        }

        public bool CanSpend(float amount)
        {
            return amount > 0 && Balance >= amount;
        }

        public void Spend(float amount, bool onCompany = false, bool onSelf = false)
        {
            if(!CanSpend(amount))
                return;
            
            Balance -= amount;

            if (onCompany)
                SpentMoneyOnCompany += amount;
            if (onSelf)
                SpentMoneyOnSelf += amount;
        }

        public void OnLoad()
        {
            IsActive = false;
            WorkPower = Mathf.Max(Common.InintialWorkPower, WorkPower);
        }
    }
}