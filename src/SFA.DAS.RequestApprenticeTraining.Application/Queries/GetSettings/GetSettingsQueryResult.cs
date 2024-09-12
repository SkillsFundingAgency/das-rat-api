namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSettings
{
    public class GetSettingsQueryResult
    {
        public int ExpiryAfterMonths { get; set; }
        public int EmployerRemovedAfterExpiryMonths { get; set; }
        public int ProviderRemovedAfterRequestedMonths { get; set; }
    }
}
