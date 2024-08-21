using System;
using System.Collections.Generic;
using System.Linq;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class EmployerRequest
    {
        public Guid Id { get; set; }
        public Enums.RequestType RequestType { get; set; }
        public long AccountId { get; set; }
        public string StandardReference { get; set; }
        public int NumberOfApprentices { get; set; }
        public string SameLocation { get; set; }
        public string SingleLocation { get; set; }
        public bool AtApprenticesWorkplace { get; set; }
        public bool DayRelease { get; set; }
        public bool BlockRelease { get; set; }
        public Guid RequestedBy { get; set; }
        public DateTime RequestedAt { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public Guid ModifiedBy { get; set; }

        public List<Region> Regions { get; set; }

        public static implicit operator EmployerRequest(Entities.EmployerRequest source)
        {
            if (source == null)
            {
                return null;
            }

            return new EmployerRequest
            {
                Id = source.Id,
                RequestType = source.RequestType,
                AccountId = source.AccountId,
                StandardReference = source.StandardReference,
                NumberOfApprentices = source.NumberOfApprentices,
                SameLocation = source.SameLocation,
                SingleLocation = source.SingleLocation,
                AtApprenticesWorkplace = source.AtApprenticesWorkplace,
                DayRelease = source.DayRelease,
                BlockRelease = source.BlockRelease,
                RequestedBy = source.RequestedBy,
                RequestedAt = source.RequestedAt,
                RequestStatus = source.RequestStatus,
                ExpiredAt = source.ExpiredAt,
                CancelledAt = source.CancelledAt,
                ModifiedBy = source.ModifiedBy,
                Regions = source.GetRegions().Select(s => (Region)s).ToList()
            };
        }
    }
}