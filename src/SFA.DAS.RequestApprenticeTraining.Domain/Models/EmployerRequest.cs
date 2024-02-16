using System;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class EmployerRequest
    {
        public Guid Id { get; set; }
        public int RequestTypeId { get; set; }

        public static implicit operator EmployerRequest(Entities.EmployerRequest source)
        {
            if (source == null)
            {
                return null;
            }

            return new EmployerRequest
            {
                Id = source.Id,
                RequestTypeId = source.RequestTypeId
            };
        }
    }
}
