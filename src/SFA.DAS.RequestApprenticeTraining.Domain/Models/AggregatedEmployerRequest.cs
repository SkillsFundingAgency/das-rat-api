using System;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class AggregatedEmployerRequest
    {
        public string CourseReference { get; set; }
        public string CourseTitle { get; set; }
        public int Level { get; set; }
        public string Sector { get; set; }
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
                CourseReference = source.CourseReference,
                CourseTitle = source.CourseTitle,
                Level = source.Level,
                Sector = source.Sector,
                NumberOfApprentices = source.NumberOfApprentices,   
                NumberOfEmployers = source.NumberOfEmployers
                
            };
        }
    }
}
