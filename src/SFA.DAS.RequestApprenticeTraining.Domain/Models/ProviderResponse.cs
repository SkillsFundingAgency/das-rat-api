using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Linq;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Models
{
    public class ProviderResponse
    {
        public long Ukprn { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public DateTime RespondedAt { get; set; }
        public Guid RespondedBy { get; set; }

        public static implicit operator ProviderResponse(Entities.ProviderResponse source)
        {
            if (source == null)
            {
                return null;
            }

            return new ProviderResponse
            {
                Ukprn = source.ProviderResponseEmployerRequests.FirstOrDefault()?.Ukprn ?? 0,
                ContactName = source.ContactName,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                Website = source.Website,
                RespondedAt = source.RespondedAt,
                RespondedBy = source.RespondedBy
            };
        }
    }
}
