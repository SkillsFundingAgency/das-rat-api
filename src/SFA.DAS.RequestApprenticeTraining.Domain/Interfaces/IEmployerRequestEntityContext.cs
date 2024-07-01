using Microsoft.EntityFrameworkCore;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Interfaces
{
    public interface IEmployerRequestEntityContext : IEntityContext<EmployerRequest>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public async Task<EmployerRequest> GetForEmployerRequestId(Guid employerRequestId)
            => await Entities
                .FirstOrDefaultAsync(er => er.Id == employerRequestId);

        public async Task<EmployerRequest> GetWithRegionsForEmployerRequestId(Guid employerRequestId)
            => await Entities
                .Include(er => er.EmployerRequestRegions)
                .ThenInclude(err => err.Region)
                .FirstOrDefaultAsync(er => er.Id == employerRequestId);

        public async Task<List<EmployerRequest>> GetForAccountId(long accountId)
            => await Entities
                .Where(er => er.AccountId == accountId)
                .ToListAsync();

        public async Task<List<EmployerRequest>> GetWithRegionsForAccountId(long accountId)
            => await Entities
                .Include(er => er.EmployerRequestRegions)
                .ThenInclude(err => err.Region)
                .Where(er => er.AccountId == accountId)
                .ToListAsync();

        public async Task<EmployerRequest> GetFirstOrDefault()
            => await Entities
                .FirstOrDefaultAsync();
    }
}
