using System.Collections.Generic;
using Twitch;
using UnityEngine;
using Utils;
using Utils.Debugger;
using System;
using Gameplay.Voting;
using UI;

namespace Gameplay.States
{
    public class IdleState : BaseState
    {
        public IdleState(GameContext context, Client client, UserDb db) : base(context, client, db) {}

        public override void StateStarted()
        {
            SoundBank.Instance.PlayOfficeAmbience();
        }
        
        public override GameState? StateUpdate()
        {
            /*
            КАЖДЫЙ ФРЕЙМ:             
             
            1. обновляем таймер зарплаты
            2. если таймер закончился
                отнимает деньги у компании (отниманием Company.расходы из Company.balance)
                
            3. проверка на банкроство
                если денег меньше 0 - переходим в Bankruptcy   
                
            4. если контракта нет  ?????
                переходим в ContractStart
                
            5. если есть контракт - обновляем его таймер
                если таймер контракта < 0
                    переходим в ContractEnd
            
            6. ** принимаем типы **
            7. ** принимаем бусты **
            
            8. ** Обновляем UI **
                таймер контракта
                таймер до события
                таймер до зарплаты 
             */

            if (Context.Company == null)
                return GameState.CompanyStart;
            
            
            Context.Company.ExpensesTimer.Update();
            if (!Context.Company.ExpensesTimer.IsActive)
            {
                Context.Company.Balance -= Context.Company.Expenses;
                SoundManager.Instance.Play(SoundBank.Instance.SalaryExpenses);
                
                Context.Company.ExpensesTimer.Reset();
            }
            
            if(Context.Company.Balance <= 0f)
                return GameState.Bankruptcy;
            
            if (!Context.Company.HasContract)
                return GameState.ChooseContractStart;

            var activeContract = Context.Company.ActiveContract;
            var workVotingState = activeContract.WorkVotingState;
            if (workVotingState == null)
                return GameState.ChooseContractStart;
            
            workVotingState.Update();
            if (workVotingState.CanBeFinished())
            {
                workVotingState.Finish(Context);
                if (workVotingState.IsSuccessful())
                {
                    Client.SendMessage("We made it! Everybody involved got a boost in their work power!");
                    return GameState.ContractEnd;
                }
                else
                {
                    Client.SendMessage("We failed to make a project. Let's try new one!");
                    return GameState.ChooseContractStart;
                }
            }
            
            return null;
        }

        public override void StateEnded()
        {
            SoundBank.Instance.StopOfficeAmbience();
        }
    }
}