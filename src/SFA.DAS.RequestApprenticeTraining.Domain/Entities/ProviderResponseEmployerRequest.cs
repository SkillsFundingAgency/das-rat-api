using System;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class ProviderResponseEmployerRequest
    {
        public Guid EmployerRequestId { get; set; }
        public long Ukprn { get; set; }
        public Guid? ProviderResponseId { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public Guid? AcknowledgedBy { get; set; }

        public EmployerRequest EmployerRequest { get; set; }
        public ProviderResponse ProviderResponse { get; set; }
    }
}
