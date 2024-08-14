using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class ProviderResponseEmployerRequest
    {
        public Guid EmployerRequestId { get; set; }
        public long Ukprn { get; set; }

        public Guid ModifiedBy { get; set; }

        public EmployerRequest EmployerRequest { get; set; }
    }
}
