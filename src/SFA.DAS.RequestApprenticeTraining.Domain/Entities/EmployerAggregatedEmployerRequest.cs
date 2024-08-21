using System;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class EmployerAggregatedEmployerRequest
    {
        public Guid EmployerRequestId { get; set; }
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public DateTime RequestedAt { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public int NumberOfResponses { get; set; }
        public int NewNumberOfResponses { get; set; }
    }
}
