namespace Gameplay
{
    public static class Common
    {
        // Config
        public const float UserTimeout = 60 * 10; // sec
        public const float SlowUpdateDelay = 10f; // sec
        
        // Resources
        public const string EventsResourcePath = "events";
        public const string ContractsResourcePath = "contracts";
        public const string CompanyNamesPath = "company_names";
        public const string DatabasePath = "users.json";

        // Gameplay
        public const int ContractsTillNextEvent = 1;
        public const int BaseOfficeLevel = 2;
        
        // Company
        public const float InitialCompanyBalance = 10000f;
        public const float BankruptcyGatherTarget = 1000f;
        public const float CEOPrice = 100000f;
        public const float BankruptcyDuration = 30f;
        public static readonly float[] BaseSalaryExpenses = {5000f, 20000f, 40000f};
        public const float TimeToSalary = 300f; // sec
        
        // User
        public const float InitialUserBalance = 1000f;
        
        // Contract
        public const int DefaultContractLevel = 1;
        public const float InintialWorkPower = 200f;
        public const float WorkPowerBoost = 10f;
        public const float DefaultVotingDuration = 30f;
        public const float DefaultContractDuration = 90f;
        public const float DefaultContractReward = 10000f;
        public const float DefaultBoostPrice = 5000f;
        public const float DefaultBoostDuration = 10f;
        public const float MoneyToWorkPointsRatio = 10f;  // Freelance

        // Tip info
        public const float DefaultTipValue = 50f;
        public const float DefaultTipStep = 10f;

        // Сколько секунд до зарплатных выплат
        public const float InformationPopupDisplayTime = 10f; // sec

        // DistVote
        public const float DistrStep = 5f;

        // Text
        public const string WorkTitle = "Time to work!";
        public const string WorkDescription = "!work - to spend your work power points\n !pay X - to hire freelancers (10$ for point)";
    }
}