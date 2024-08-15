namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSettings
{
    public class GetSettingsQueryResult
    {
        public int ExpiryAfterMonths { get; set; }
        public int RemovedAfterExpiryNotContactedMonths { get; set; }
        public int RemovedAfterExpiryContactedMonths { get; set; }
    }
}
