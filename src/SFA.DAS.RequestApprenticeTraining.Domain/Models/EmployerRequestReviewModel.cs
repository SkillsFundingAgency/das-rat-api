using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class EmployerRequestReviewModel
    {
        public Guid EmployerRequestId { get; set; }
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public DateTime DateOfRequest { get; set; }
        public int NumberOfApprentices { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public List<string> Locations { get; set; }

        public static implicit operator EmployerRequestReviewModel(Entities.EmployerRequestReviewModel source)
        {
            return new EmployerRequestReviewModel
            {
                EmployerRequestId = source.EmployerRequestId,
                StandardReference = source.StandardReference,
                StandardTitle = source.StandardTitle,
                StandardLevel = source.StandardLevel,
                DateOfRequest = source.DateOfRequest,
                NumberOfApprentices = source.NumberOfApprentices,
                DayRelease = source.DayRelease,
                BlockRelease = source.BlockRelease,
                AtApprenticesWorkplace = source.AtApprenticesWorkplace,
                Locations = source.Locations,
            };
        }
    }
}
