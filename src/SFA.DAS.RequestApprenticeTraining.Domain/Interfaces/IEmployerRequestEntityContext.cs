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

        public async Task<EmployerRequest> Get(Guid employerRequestId)
            => await Entities
                .Include(er => er.EmployerRequestRegions)
                .ThenInclude(err => err.Region)
                .FirstOrDefaultAsync(er => er.Id == employerRequestId && er.RequestStatus == Models.Enums.RequestStatus.Active);

        public async Task<EmployerRequest> Get(long accountId, string standardReference)
            => await Entities
                .Include(er => er.EmployerRequestRegions)
                .ThenInclude(err => err.Region)
                .SingleOrDefaultAsync(er => er.AccountId == accountId && er.StandardReference == standardReference && er.RequestStatus == Models.Enums.RequestStatus.Active);

        public async Task<List<EmployerRequest>> Get(long accountId)
            => await Entities
                .Include(er => er.EmployerRequestRegions)
                .ThenInclude(err => err.Region)
                .Where(er => er.AccountId == accountId)
                .ToListAsync();

        public async Task<EmployerRequest> GetFirstOrDefault()
            => await Entities
                .FirstOrDefaultAsync();

        public async Task<List<Models.AggregatedEmployerRequest>> GetAggregatedEmployerRequests(long ukprn)
        {
            var result = await Entities
                .Where(er => er.Status == Models.Enums.Status.Active)
                .GroupBy(er => new { er.StandardReference, er.Standard.StandardTitle, er.Standard.StandardLevel, er.Standard.StandardSector })
                .Select(g => new
                {
                    g.Key.StandardReference,
                    NumberOfApprentices = g.Sum(x => x.NumberOfApprentices),
                    NumberOfEmployers = g.Count(),
                    g.Key.StandardTitle,
                    g.Key.StandardLevel,
                    g.Key.StandardSector,
                    IsNew = g.Any(er => !er.ProviderResponseEmployerRequests.Any(pre => pre.Ukprn == ukprn))
                })
                .OrderBy(x => x.StandardReference)
                .Select(x => new Models.AggregatedEmployerRequest
                {
                    StandardReference = x.StandardReference,
                    StandardTitle = x.StandardTitle,
                    StandardLevel = x.StandardLevel,
                    StandardSector = x.StandardSector,
                    NumberOfApprentices = x.NumberOfApprentices,
                    NumberOfEmployers = x.NumberOfEmployers,
                    IsNew = x.IsNew
                })
                .ToListAsync();

            return result;
        }
    }
}
