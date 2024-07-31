using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class SelectEmployerRequest
    {
        public Guid EmployerRequestId { get; set; }
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string SingleLocation { get; set; }
        public DateTime DateOfRequest { get; set; }
        public int NumberOfApprentices { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool IsNew { get; set; }
        public bool IsContacted { get; set; }
        public List<string> Locations { get; set; }

        public static implicit operator SelectEmployerRequest(Entities.SelectEmployerRequest source)
        {
            return new SelectEmployerRequest
            {
                EmployerRequestId = source.EmployerRequestId,
                StandardReference = source.StandardReference,
                StandardTitle = source.StandardTitle,
                StandardLevel = source.StandardLevel,
                SingleLocation = source.SingleLocation,
                DateOfRequest = source.DateOfRequest,   
                NumberOfApprentices = source.NumberOfApprentices,
                DayRelease = source.DayRelease,
                BlockRelease = source.BlockRelease,
                AtApprenticesWorkplace = source.AtApprenticesWorkplace,
                IsNew = source.IsNew,
                IsContacted = source.IsContacted,
                Locations = source.Locations,
            };
        }
    }
}
