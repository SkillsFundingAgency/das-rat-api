using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class SubmitProviderResponseParameters
    {
        public List<Guid> EmployerRequestIds { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string ContactName { get; set; }
        public Guid RespondedBy { get; set; }


    }
}
