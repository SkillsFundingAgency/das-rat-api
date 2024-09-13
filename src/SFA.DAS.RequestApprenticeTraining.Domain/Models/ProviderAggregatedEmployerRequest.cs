namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class ProviderAggregatedEmployerRequest
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }
        public int NumberOfApprentices { get; set; }
        public int NumberOfEmployers { get; set; }
        public bool IsNew { get; set; }

        public static implicit operator ProviderAggregatedEmployerRequest(Entities.ProviderAggregatedEmployerRequest source)
        {
            return new ProviderAggregatedEmployerRequest
            {
                IsNew = source.IsNew,
                StandardReference = source.StandardReference,   
                StandardTitle = source.StandardTitle,   
                StandardLevel = source.StandardLevel,
                StandardSector = source.StandardSector,
                NumberOfApprentices = source.NumberOfApprentices,   
                NumberOfEmployers = source.NumberOfEmployers
            };
        }
    }
}
