using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Entities
{
    public class EmployerRequest
    {
        public Guid Id { get; set; }
        public Enums.RequestType RequestType { get; set; }
    }
}
