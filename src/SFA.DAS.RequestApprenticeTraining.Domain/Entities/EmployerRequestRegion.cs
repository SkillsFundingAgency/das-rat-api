using System;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class EmployerRequestRegion
    {
        public Guid EmployerRequestId { get; set; }
        public int RegionId { get; set; }
        public Guid ModifiedBy { get; set; }

        public EmployerRequest EmployerRequest { get; set; }

        public Region Region { get; set; }
    }
}
