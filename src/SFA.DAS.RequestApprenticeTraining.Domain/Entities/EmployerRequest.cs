using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System;
using System.Collections.Generic;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class EmployerRequest
    {
        public Guid Id { get; set; }
        public string OriginalLocation { get; set; }
        public Enums.RequestType RequestType { get; set; }
        public long AccountId { get; set; }
        public string StandardReference { get; set; }
        public int NumberOfApprentices { get; set; }
        public string SingleLocation { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public Guid RequestedBy { get; set; }
        public Status Status { get; set; }
        public Guid ModifiedBy { get; set; }
        public virtual Standard Standard { get; set; }
        public virtual List<EmployerRequestRegion> EmployerRequestRegions { get; set; }
    }
}
