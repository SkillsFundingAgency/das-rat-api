using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public string SameLocation { get; set; }
        public string SingleLocation { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public DateTime RequestedAt { get; set; }
        public Guid RequestedBy { get; set; }
        public Enums.RequestStatus RequestStatus { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public Guid ModifiedBy { get; set; }

        public List<EmployerRequestRegion> EmployerRequestRegions { get; set; }
        public List<ProviderResponseEmployerRequest> ProviderResponseEmployerRequests { get; set; }
      
        public List<Region> GetRegions()
        {
            return EmployerRequestRegions.Select(err => err.Region).ToList();
        }
        public Standard Standard { get; set; }
        
    }
}
