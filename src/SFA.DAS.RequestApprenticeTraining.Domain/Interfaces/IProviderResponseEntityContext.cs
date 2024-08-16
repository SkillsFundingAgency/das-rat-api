using Microsoft.EntityFrameworkCore;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Interfaces
{
    public interface IProviderResponseEntityContext : IEntityContext<ProviderResponse>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public async Task<List<ProviderResponse>> Get()
            => await Entities.ToListAsync();

        public async Task<ProviderResponse> Get(Guid providerResponseId)
            => await Entities
                .Include(pr => pr.ProviderResponseEmployerRequests)
                .FirstOrDefaultAsync(pr => pr.Id == providerResponseId);

    }
}
