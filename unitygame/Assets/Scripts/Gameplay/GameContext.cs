using System.Collections.Generic;
using System.Linq;
using Gameplay.Lang;
using Messages;
using Twitch;
using Utils;

namespace Gameplay
{
    public class GameContext
    {
        public Company Company
        {
            get => _company;
            set
            {
                _company = value;
                EventBus.Publish(new CompanyChanged(this, value));
            }
        }
        
        public readonly UserDb Db;
        public readonly Client Client;
        public readonly List<EventInfo> Events;
        public readonly List<ContractInfo> Contracts;
        public int ContractsTillNextEvent;
        
        public string SelectedOption;

        private Company _company;

        public GameContext(Company company, UserDb db, Client client)
        {
            Db = db;
            Client = client;
            Company = company;
            Events = Registry.LoadEvents(Common.EventsResourcePath);
            Contracts = Registry.LoadContracts(Common.ContractsResourcePath);
            ContractsTillNextEvent = Common.ContractsTillNextEvent;
        }

        public float? GetParameterValue(string parameter)
        {
            switch (parameter)
            {
                case "balance":
                    return Company.Balance;
                case "specials":
                    return Company.Specials;
            }

            return null;
        }

        
        public static readonly HashSet<string> AvailableParams = new HashSet<string>()
        {
            "balance",
            "specials",
            "marketing",
            "technology",
            "relevance"
        };

        public EventInfo GetRandomAvailableEvent()
        {
            var availableEvents = Events.Where(e => e.IsAvailable(this)).ToList();
            return RandomUtils.Choice(availableEvents);
        }
    }
}