using System;
using System.Collections.Generic;
using Gameplay.Lang.Actions;
using Gameplay.Lang.Conditions;
using Gameplay.Lang.Votings;
using SimpleJSON;
using UnityEngine;

namespace Gameplay.Lang
{
    public static class Registry
    {
        public static ICondition MakeCondition(string type, JSONObject args)
        {
            try
            {
                switch (type)
                {
                    case "and": return new AndCondition(args);
                    case "or": return new OrCondition(args);
                    case "not": return new NotCondition(args);
                    case "param_in_range": return new ParameterInRangeCondition(args);
                    case "top_player_has_money": return new TopPlayerHasMoneyCondition(args);
                    case "has_flag": return new HasFlagCondition(args);
                    default:
                        throw new KeyNotFoundException($"No such condition type \"{type}\"");
                }   
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError($"Cant create condition of type: \"{type}\" with args {args} check your code!");
                throw;
            }
        }
        
        public static IAction MakeAction(string type, JSONObject args)
        {
            try
            {
                switch (type)
                {
                    case "composite": return new ActionsAction(args);
                    case "send_message": return new SendMessageAction(args);
                    case "conditional": return new ConditionalAction(args);
                    
                    case "mod_balance": return new ModBalance(args);
                    case "mod_specials": return new ModSpecials(args);
                    case "mod_salary": return new ModSalary(args);

                    case "mod_players_balance": return new ModPlayersBalance(args);
                    case "mod_top_player_balance": return new ModTopPlayerBalance(args);
                    case "mod_worst_player_balance": return new ModWorstPlayerBalance(args);
                    case "mod_selected_player_balance": return new ModSelectedPlayerBalance(args);
                    case "mod_random_player_balance": return new ModRandomPlayerBalance(args);
                    case "grade_office": return new GradeOffice(args);
                    case "boost_param": return new BoostParam(args);
                    
                    case "set_flag": return new SetFlag(args);
                    case "reset_flag": return new ResetFlag(args);

                    default:
                        throw new KeyNotFoundException($"No such action type \"{type}\"");
                }   
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError($"Cant create action of type: \"{type}\" with args {args} check your code!");
                throw;
            }
        }
        
        public static IEventVoting MakeVoting(string type, JSONObject args)
        {
            try
            {
                switch (type)
                {
                    case "options": return new OptionsVotingInfo(args);
                    case "money": return new MoneyVotingInfo(args);
                    default:
                        throw new KeyNotFoundException($"No such voting type \"{type}\"");
                }   
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError($"Cant create voting of type: \"{type}\" with args {args} check your code!");
                throw;
            }
        }

        public static IAction MakeAction(JSONObject args)
        {
            if (args == null)
                return null;
            
            if (!args.HasKey("type"))
                throw new KeyNotFoundException($"Missing \"type\" in {args}");

            var type = args["type"].Value;
            return MakeAction(type, args);
        }
        
        public static ICondition MakeCondition(JSONObject args)
        {
            if (args == null)
                return null;
            
            if (!args.HasKey("type"))
                throw new KeyNotFoundException($"Missing \"type\" in {args}");

            var type = args["type"].Value;
            return MakeCondition(type, args);
        }

        public static IEventVoting MakeVoting(JSONObject args)
        {
            if (args == null)
                return null;
            
            if (!args.HasKey("type"))
                throw new KeyNotFoundException($"Missing \"type\" in {args}");

            var type = args["type"].Value;
            return MakeVoting(type, args);
        }

        public static List<EventInfo> LoadEvents(string path)
        {
            var items = new List<EventInfo>();
            var asset = Resources.Load<TextAsset>(path);
            var result = JSON.Parse(asset.text);
            foreach (var item in result.AsArray)
            {
                items.Add(new EventInfo(item.Value.AsObject));
            }
            
            Debug.Log($"[Registry] Loaded {items.Count} events from {path}");
            return items;
        }
        
        public static List<ContractInfo> LoadContracts(string path)
        {
            var items = new List<ContractInfo>();
            var asset = Resources.Load<TextAsset>(path);
            var result = JSON.Parse(asset.text);
            foreach (var item in result.AsArray)
            {
                items.Add(new ContractInfo(item.Value.AsObject));
            }
            
            Debug.Log($"[Registry] Loaded {items.Count} contracts from {path}");
            return items;
        }
        
        public static List<string> LoadListOfStrings(string path)
        {
            var items = new List<string>();
            var asset = Resources.Load<TextAsset>(path);
            var result = JSON.Parse(asset.text);
            foreach (var item in result.AsArray)
            {
                items.Add(item.Value);
            }
            
            Debug.Log($"[Registry] Loaded {items.Count} items from {path}");
            return items;
        }
    }
}