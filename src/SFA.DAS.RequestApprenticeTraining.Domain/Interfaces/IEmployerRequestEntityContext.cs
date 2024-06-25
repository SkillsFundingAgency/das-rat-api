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

        public async Task<List<EmployerRequest>> GetForAccountId(long accountId)
            => await Entities
                .Where(er => er.AccountId == accountId)
                .ToListAsync();

        public async Task<EmployerRequest> GetFirstOrDefault()
            => await Entities
                .FirstOrDefaultAsync();

        public async Task<List<AggregatedEmployerRequest>> GetAggregatedEmployerRequests()
        {
            var result = await Entities
                .GroupBy(er => new { er.StandardReference, er.Standard.StandardTitle, er.Standard.StandardLevel, er.Standard.StandardSector })
                .Select(g => new AggregatedEmployerRequest
                {
                    StandardReference = g.Key.StandardReference,
                    NumberOfApprentices = g.Sum(x => x.NumberOfApprentices),
                    NumberOfEmployers = g.Count(),
                    StandardTitle = g.Key.StandardTitle,
                    StandardLevel = g.Key.StandardLevel,
                    StandardSector = g.Key.StandardSector
                })
                .OrderBy(x => x.StandardReference)
                .ToListAsync();

            return result;
        }
    }
}
