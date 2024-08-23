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

        public async Task<EmployerRequest> GetWithRegions(Guid employerRequestId)
            => await Entities
                .Include(er => er.EmployerRequestRegions)
                .ThenInclude(err => err.Region)
                .FirstOrDefaultAsync(er => er.Id == employerRequestId && er.RequestStatus == Models.Enums.RequestStatus.Active);

        public async Task<EmployerRequest> GetWithRegions(long accountId, string standardReference)
            => await Entities
                .Include(er => er.EmployerRequestRegions)
                .ThenInclude(err => err.Region)
                .SingleOrDefaultAsync(er => er.AccountId == accountId && er.StandardReference == standardReference && er.RequestStatus == Models.Enums.RequestStatus.Active);

        public async Task<List<EmployerRequest>> GetWithRegions(long accountId)
            => await Entities
                .Include(er => er.EmployerRequestRegions)
                .ThenInclude(err => err.Region)
                .Where(er => er.AccountId == accountId)
                .ToListAsync();

        public async Task<EmployerRequest> GetWithResponses(Guid employerRequestId)
            => await Entities
                .Include(er => er.ProviderResponseEmployerRequests)
                .ThenInclude(prer => prer.ProviderResponse)
                .FirstOrDefaultAsync(er => er.Id == employerRequestId && er.RequestStatus == Models.Enums.RequestStatus.Active);

        public async Task<EmployerRequest> GetFirstOrDefault()
            => await Entities
                .FirstOrDefaultAsync();

        public async Task<List<EmployerAggregatedEmployerRequest>> GetEmployerAggregatedEmployerRequests(long accountId)
        {
            var result = await Entities
                .Include(er => er.ProviderResponseEmployerRequests)
                .Where(er => er.RequestStatus != Models.Enums.RequestStatus.Cancelled && er.AccountId == accountId)
                .SelectMany(er => er.ProviderResponseEmployerRequests.DefaultIfEmpty(), (er, prer) => new
                {
                    er.Id,
                    er.StandardReference,
                    er.Standard.StandardTitle,
                    er.Standard.StandardLevel,
                    er.RequestedAt,
                    er.RequestStatus,
                    IsResponseProvided = prer.ProviderResponse != null,
                    IsNewResponse = prer.ProviderResponse != null && prer.AcknowledgedAt == null
                })
                .GroupBy(er => new { er.Id, er.StandardReference, er.StandardTitle, er.StandardLevel, er.RequestedAt, er.RequestStatus })
                .Select(g => new EmployerAggregatedEmployerRequest
                {
                    EmployerRequestId = g.Key.Id,
                    StandardReference = g.Key.StandardReference,
                    StandardTitle = g.Key.StandardTitle,
                    StandardLevel = g.Key.StandardLevel,
                    RequestedAt = g.Key.RequestedAt,
                    RequestStatus = g.Key.RequestStatus,
                    NumberOfResponses = g.Count(x => x.IsResponseProvided),
                    NewNumberOfResponses = g.Count(x => x.IsNewResponse)
                })
                .OrderBy(x => x.StandardTitle)
                .ToListAsync();

            return result;
        }

        public async Task<List<ProviderAggregatedEmployerRequest>> GetProviderAggregatedEmployerRequests(long ukprn)
        {
            var result = await Entities
                .Where(er => er.RequestStatus == Models.Enums.RequestStatus.Active)
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
                .Select(x => new ProviderAggregatedEmployerRequest
                {
                    StandardReference = x.StandardReference,
                    StandardTitle = x.StandardTitle,
                    StandardLevel = x.StandardLevel,
                    StandardSector = x.StandardSector,
                    NumberOfApprentices = x.NumberOfApprentices,
                    NumberOfEmployers = x.NumberOfEmployers,
                    IsNew = x.IsNew,
                })
                .OrderBy(x => x.StandardTitle)
                .ToListAsync();

            return result;
        }

        public async Task<List<SelectEmployerRequest>> GetForProviderStandard(long ukprn, string standardReference)
        {
            var result = await Entities
                .Where(er => er.StandardReference == standardReference && er.RequestStatus == Models.Enums.RequestStatus.Active)
                .Select(er => new SelectEmployerRequest
                {
                    EmployerRequestId = er.Id,
                    StandardReference = er.StandardReference,
                    StandardTitle = er.Standard.StandardTitle,
                    StandardLevel = er.Standard.StandardLevel,
                    DateOfRequest = DateTime.Now,
                    DayRelease = er.DayRelease,
                    BlockRelease = er.BlockRelease,
                    AtApprenticesWorkplace = er.AtApprenticesWorkplace,
                    SingleLocation = er.SingleLocation,
                    NumberOfApprentices= er.NumberOfApprentices,
                    Locations = er.EmployerRequestRegions.Select(requestRegion => requestRegion.Region.SubregionName).ToList(),
                    IsNew = !er.ProviderResponseEmployerRequests.Any(pre => pre.Ukprn == ukprn),
                })
                .OrderBy(x => x.StandardTitle)
                .ToListAsync();

            return result;
        }

    }
}
