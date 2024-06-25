using System;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class AggregatedEmployerRequest
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }
        public int NumberOfApprentices { get; set; }
        public int NumberOfEmployers { get; set; }

        public static implicit operator AggregatedEmployerRequest(Entities.AggregatedEmployerRequest source)
        {
            if (source == null)
            {
                return null;
            }

            return new AggregatedEmployerRequest
            {
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
