using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class EmployerRequest : EntityBase
    {
        public Guid Id { get; set; }
        public Enums.RequestType RequestType { get; set; }
        public long AccountId { get; set; }
    }
}
