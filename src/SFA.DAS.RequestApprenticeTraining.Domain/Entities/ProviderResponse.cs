using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class ProviderResponse
    {
        public Guid Id { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public DateTime RespondedAt { get; set; }
        public Guid RespondedBy { get; set; }
        public List<ProviderResponseEmployerRequest> ProviderResponseEmployerRequests { get; set; }
    }
}
