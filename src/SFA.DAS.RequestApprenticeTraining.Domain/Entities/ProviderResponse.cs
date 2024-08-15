using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class ProviderResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public DateTime RespondedAt { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
    }
}
