using Microsoft.EntityFrameworkCore;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Interfaces
{
    public interface IProviderResponseEmployerRequestEntityContext : IEntityContext<ProviderResponseEmployerRequest>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public async Task<bool> CreateIfNotExistsAsync(ProviderResponseEmployerRequest response) 
        {
            bool exists = await Entities.AnyAsync(s => s.Ukprn == response.Ukprn && s.EmployerRequestId == response.EmployerRequestId);

            if (!exists)
            {
                await Entities.AddAsync(response);
                return true; 
            }
            return false;
        }
    }
}
