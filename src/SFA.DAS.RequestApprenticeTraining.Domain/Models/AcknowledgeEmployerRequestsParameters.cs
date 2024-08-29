using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public  class AcknowledgeEmployerRequestsParameters
    {
        public List<Guid> EmployerRequestIds { get; set; }
    }
}
