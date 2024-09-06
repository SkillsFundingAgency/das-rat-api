using System;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class EmployerRequestForResponseNotification
    {
        public Guid RequestedBy { get; set; }
        public long AccountId { get; set; }
        public Guid EmployerRequestId { get; set; }
        public List<StandardDetails> Standard { get; set; }
    }

    public class StandardDetails
    {
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
    }
}
