using System;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class EmployerAggregatedEmployerRequest
    {
        public Guid EmployerRequestId { get; set; }
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime ExpiryAt { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public int NumberOfResponses { get; set; }
        public int NewNumberOfResponses { get; set; }

        public static implicit operator EmployerAggregatedEmployerRequest(Entities.EmployerAggregatedEmployerRequest source)
        {
            return new EmployerAggregatedEmployerRequest
            {
                EmployerRequestId = source.EmployerRequestId,
                StandardReference = source.StandardReference,
                StandardTitle = source.StandardTitle,
                StandardLevel = source.StandardLevel,
                RequestedAt = source.RequestedAt,
                RequestStatus = source.RequestStatus,
                NumberOfResponses = source.NumberOfResponses,
                NewNumberOfResponses = source.NewNumberOfResponses
            };
        }
    }
}
