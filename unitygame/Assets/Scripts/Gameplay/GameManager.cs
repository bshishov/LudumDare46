using System.Collections;
using System.Collections.Generic;
using Gameplay.States;
using Messages;
using Twitch;
using UI;
using UnityEngine;
using Utils;
#if DEBUGGER
using Utils.Debugger;
#endif
using Utils.FSM;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class GameManager : MonoBehaviour
    {
        private Client _client;
        private UserDb _db;
        private StateMachine<GameState> _stateMachine;
        private GameContext _context;
        
        private void Start()
        {   
            Application.runInBackground = true;
            
            _db = new UserDb();
            _db.Load(Common.DatabasePath);
            
            _client = new Client(_db);
            _client.OnUserCommand += OnUserCommand;
            _client.OnUserMessage += OnUserMessage;
            
            _context = new GameContext(null, _db, _client);
            
            _stateMachine = new StateMachine<GameState>();
            
            _stateMachine.AddState(GameState.Idle, new IdleState(_context, _client, _db));
            _stateMachine.AddState(GameState.CompanyStart, new CompanyStartState(_context, _client, _db));
            _stateMachine.AddState(GameState.ContractStart, new ContractStartState(_context, _client, _db));
            _stateMachine.AddState(GameState.ContractEnd, new ContractEndState(_context, _client, _db));
            _stateMachine.AddState(GameState.Event, new EventVotingState(_context, _client, _db));
            _stateMachine.AddState(GameState.Bankruptcy, new BankruptcyState(_context, _client, _db));
            _stateMachine.AddState(GameState.ChooseContractStart, new ChooseContractState(_context, _client, _db));

            _stateMachine.StateChanged += state => EventBus.Publish(new GameStateChanged(this, state)); 
            

            _stateMachine.SwitchToState(GameState.Idle);
            
            // Debug switches
            #if DEBUGGER
            Debugger.Default.Display("GameState/Idle", () => _stateMachine.SwitchToState(GameState.Idle));
            Debugger.Default.Display("GameState/CompanyStart", () => _stateMachine.SwitchToState(GameState.CompanyStart));
            Debugger.Default.Display("GameState/ContractStart", () => _stateMachine.SwitchToState(GameState.ContractStart));
            Debugger.Default.Display("GameState/ContractEnd", () => _stateMachine.SwitchToState(GameState.ContractEnd));
            Debugger.Default.Display("GameState/EventVoting", () => _stateMachine.SwitchToState(GameState.Event));
            Debugger.Default.Display("GameState/Bankruptcy", () => _stateMachine.SwitchToState(GameState.Bankruptcy));
            
            Debugger.Default.Display("Reset user balances & wp", () =>
            {
                foreach (var user in _db.GetAllUsers())
                {
                    user.Balance = Common.InitialUserBalance;
                    user.WorkPower = Common.InintialWorkPower;
                }
            });
            
            Debugger.Default.Display("Office/Level 2", () => _context.Company.Level = 2);
            Debugger.Default.Display("Office/Level 3", () => _context.Company.Level = 3);
            Debugger.Default.Display("Office/Level 4", () => _context.Company.Level = 4);
            #endif

            StartCoroutine(CoroutineUtils.SlowUpdate(SlowUpdate, Common.SlowUpdateDelay));
        }

        private void Update()
        {
            _stateMachine?.Update();
            
            #if DEBUGGER
            if (_stateMachine != null)
                Debugger.Default.Display("Current State", _stateMachine.CurrentState.ToString());
            
            Debugger.Default.Display("Active Users", _context.Db.CountActive());

            if (_context != null && _context.Company != null)
            {
                var company = _context.Company;
                Debugger.Default.Display("Company/Balance", company.Balance);
                Debugger.Default.Display("Company/Balance/+ $1000", () => company.Balance += 1000);
                Debugger.Default.Display("Company/Balance/- $1000", () => company.Balance -= 1000);
                
                Debugger.Default.Display("Company/Level", company.Level);
                Debugger.Default.Display("Company/Expenses", company.Expenses);
                Debugger.Default.Display("Company/ExpensesTimer/Duration", company.ExpensesTimer.Duration);
                Debugger.Default.Display("Company/ExpensesTimer/TimeRemaining", company.ExpensesTimer.TimeRemaining);
                Debugger.Default.Display("Company/Specials", company.Specials);

                foreach (var param in company.Params)
                    Debugger.Default.Display($"Company/Params/{param.Key}", param.Value);
                    
                Debugger.Default.Display($"Company/Flags", string.Join(", ", company.Flags));

                var contract = company.ActiveContract;
                if (contract != null)
                {
                    Debugger.Default.Display("Contract/Name", contract.Info.Name);
                    Debugger.Default.Display("Contract/Reward", contract.Info.Reward);
                    Debugger.Default.Display("Contract/Difficulty", contract.Info.TotalDifficulty);
                    Debugger.Default.Display("Contract/TimeRemaining", contract.WorkVotingState.TimeRemaining);
                }
            }
            #endif        
        }

        private void OnUserCommand(User user, string command, List<string> args)
        {
            switch (command)
            {
                case "balance":
                {
                    if (args.Parse(out string target))
                    {
                        if (user.CanCheat)
                        {
                            var targetUser = _db.GetUser(target);
                            if (targetUser != null)
                                _client.SendMessage($"{targetUser.Name}'s balance is {targetUser.Balance:N0}");
                        }
                    }
                    else
                    {
                        _client.SendMessage($"@{user.Name}, your balance is ${user.Balance:N0} and work power is {user.WorkPower}");
                    }

                    break;
                }
                case "give":
                {
                    if (user.CanCheat && args.Parse(out var target, out float amount))
                    {
                        var targetUser = _db.GetUser(target);
                        if (targetUser != null)
                            targetUser.Balance += amount;
                    }

                    break;
                }
                case "send":
                {
                    if (args.Parse(out var target, out float amount))
                    {
                        var targetUser = _db.GetUser(target);
                        if (targetUser != null && targetUser != user)
                        {
                            if (user.CanSpend(amount))
                                user.Spend(amount);
                            targetUser.Balance += amount;
                        }
                    }
                    break;
                }
                case "bet":
                {
                    if (args.Count == 2)
                    {
                        if(int.TryParse(args[0], out var amount) && amount > 0 && user.CanSpend(amount))
                        {
                            var bet = args[1];
                            if (bet == "black" || bet == "red")
                            {
                                if (Random.value < 0.5f)
                                {
                                    user.Balance += amount;
                                    _client.SendMessage($"@{user.Name} bet on {bet} and won {amount}! PogChamp");
                                }
                                else
                                {
                                    user.Spend(amount, onSelf: true);
                                    _client.SendMessage($"@{user.Name} bet on {bet} and lost {amount}! BabyRage");
                                }
                            }
                            else
                            {
                                user.Spend(amount, onSelf: true);
                                _client.SendMessage($"@{user.Name}, you can't bet on {bet}, however, you've lost the bet 4Head");
                            }
                        }
                        else if ((args[0] == "all") && user.CanSpend(user.Balance))
                        {
                            var bet = args[1];
                            if (bet == "black" || bet == "red")
                            {
                                if (Random.value < 0.5f)
                                {
                                    user.Balance += user.Balance;
                                    _client.SendMessage($"@{user.Name} bet on all his money and won! PogChamp");
                                }
                                else
                                {
                                    user.Spend(user.Balance, onSelf: true);
                                    _client.SendMessage($"@{user.Name} bet on all his money and lost! BabyRage");
                                }
                            }
                            else
                            {
                                user.Spend(amount, onSelf: true);
                                _client.SendMessage($"@{user.Name}, you can't bet on {bet}, this time we forgive you 4Head");
                            }
                        }
                    }
                    break;
                }
                case "commands":
                case "help":
                case "wtf":
                {
                    var helpText1 = @"
balance - show your account balance
!donate X - donates X amount of money
!tip tech/hype/relevance or tech/hype/relevance - improves selected area";

                    var helpText2 = @"
!boost - spend 5 000 from your personal account to speed up current contract
!vote 1/2/3/4 or 1/2/3/4 - vote toward chosen option during voting event
!bet X red/black – 50% chance to double your money or to lose";
                    
                    _client.SendMessage(helpText1.Trim());
                    _client.SendMessage(helpText2.Trim());
                    break;
                }
                case "ceo":
                {
                    if (_context.Company != null)
                    { 
                        if (user.CanSpend(Common.CEOPrice))
                        {
                            if (user == _context.Company.Ceo)
                                _client.SendMessage($"You are already CEO, lol");
                            else
                            {
                                _context.Company.Ceo = user;                                    
                                _client.SendMessage($"@{user.Name} become new CEO");
                                user.Spend(Common.CEOPrice);
                            }
                        }
                        else
                        {
                            if (_context.Company.Ceo == null)
                            {
                                _context.Company.Ceo = user;
                                _client.SendMessage($"There was no CEO, so {user.Name} become new CEO for free!");
                            }
                            else
                            {
                                if (user == _context.Company.Ceo)
                                    _client.SendMessage($"You are already CEO, lol");
                                else
                                    _client.SendMessage($"@{_context.Company.Ceo.Name} is CEO, you don't have enough money!");
                            }
                        }
                    }
                    break;
                }
                default:
                    break;
            }

            // Notify active state
            var active = _stateMachine.CurrentBehaviour;
            if (active is IUserCommandHandler baseState)
                baseState.OnUserCommand(user, command, args);
            
            
            _context.Company?.OnUserCommand(user, command, args);
        }
        
        private void OnUserMessage(User user, string message)
        {
            // Notify active state
            var active = _stateMachine.CurrentBehaviour;
            if (active is IUserMessageHandler baseState)
                baseState.OnUserMessage(user, message);
            
            _context.Company?.OnUserMessage(user, message);
        }

        private void OnDestroy()
        {
            _db.Save(Common.DatabasePath);
        }

        private void SlowUpdate(float dt)
        {
            Debug.Log($"Slow update");
            UIManager.Instance.SetTopUsers(_db.GetRichest(10));
            
            var currentTime = Time.time;
            foreach (var user in _db.GetActiveUsers())
            {
                if (currentTime - user.LastInteraction > Common.UserTimeout)
                {
                    Debug.Log($"[GameManager] User timed-out for inactivity: {user.Name}");
                    user.IsActive = false;
                }
            }
        }
    }
}