namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSettings
{
    public class GetSettingsQueryResult
    {
        public int ExpiryAfterMonths { get; set; }
        public int EmployerRemovedAfterExpiryNoResponsesMonths { get; set; }
        public int EmployerRemovedAfterExpiryResponsesMonths { get; set; }
        public int ProviderRemovedAfterExpiryNotRespondedMonths { get; set; }
        public int ProviderRemovedAfterExpiryRespondedMonths { get; set; }
    }
}
