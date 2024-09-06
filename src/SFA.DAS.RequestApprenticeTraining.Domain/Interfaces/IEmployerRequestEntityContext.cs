﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<List<AggregatedEmployerRequest>> GetAggregatedEmployerRequests(long ukprn, int providerRemovedAfterExpiryRespondedMonths)
        {
            var result = await Entities
                .Where(er =>
                    (er.Standard != null) &&
                    ((
                        er.RequestStatus == Models.Enums.RequestStatus.Active &&
                        !er.ProviderResponseEmployerRequests.Any(pre => pre.Ukprn == ukprn && pre.ProviderResponseId.HasValue)
                    ) ||
                    (
                        er.ProviderResponseEmployerRequests.Any(pre => pre.Ukprn == ukprn && pre.ProviderResponseId.HasValue) &&
                        er.RequestedAt.AddMonths(providerRemovedAfterExpiryRespondedMonths) > DateTime.Now)
                    ))
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
                .Select(x => new AggregatedEmployerRequest
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

        public async Task<List<SelectEmployerRequest>> GetForProviderStandard(long ukprn, string standardReference,int providerRemovedAfterRequestedMonths)
        {
            var result = await Entities
                .Where(er => er.Standard != null && er.StandardReference == standardReference &&
                    (
                        er.RequestStatus == Models.Enums.RequestStatus.Active && 
                        !er.ProviderResponseEmployerRequests.Any(pre => pre.Ukprn == ukprn && pre.ProviderResponseId.HasValue)
                    ) ||
                    (
                        er.ProviderResponseEmployerRequests.Any(pre => pre.Ukprn == ukprn && 
                        pre.ProviderResponseId.HasValue) && er.RequestedAt.AddMonths(providerRemovedAfterRequestedMonths) > DateTime.Now)
                    )
                .Select(er => new SelectEmployerRequest
                {
                    EmployerRequestId = er.Id,
                    StandardReference = er.StandardReference,
                    StandardTitle = er.Standard.StandardTitle,
                    StandardLevel = er.Standard.StandardLevel,
                    DateOfRequest = er.RequestedAt,
                    DayRelease = er.DayRelease,
                    BlockRelease = er.BlockRelease,
                    AtApprenticesWorkplace = er.AtApprenticesWorkplace,
                    SingleLocation = er.SingleLocation,
                    NumberOfApprentices= er.NumberOfApprentices,
                    Locations = er.EmployerRequestRegions.Select(requestRegion => requestRegion.Region.SubregionName).ToList(),
                    IsNew = !er.ProviderResponseEmployerRequests.Any(pre => pre.Ukprn == ukprn),
                    IsContacted = er.ProviderResponseEmployerRequests.Any(pre => pre.Ukprn == ukprn && pre.ProviderResponseId.HasValue),
                    DateContacted = er.ProviderResponseEmployerRequests
                       .Where(pre => pre.Ukprn == ukprn && pre.ProviderResponseId.HasValue)
                       .Select(pre => (DateTime?)pre.ProviderResponse.RespondedAt)
                       .FirstOrDefault()
                })
                .OrderByDescending(x => x.DateOfRequest)
                .ToListAsync();

            return result;
        }

        public async Task<List<EmployerRequestReviewModel>> GetByIds(List<Guid> employerRequestIds)
        {
            var result = await Entities
                .Where(er => er.Standard != null && employerRequestIds.Contains(er.Id) && er.RequestStatus == Models.Enums.RequestStatus.Active)
                .Select(er => new EmployerRequestReviewModel
                {
                    EmployerRequestId = er.Id,
                    StandardReference = er.StandardReference,
                    StandardTitle = er.Standard.StandardTitle,
                    StandardLevel = er.Standard.StandardLevel,
                    DateOfRequest = er.RequestedAt,
                    DayRelease = er.DayRelease,
                    BlockRelease = er.BlockRelease,
                    AtApprenticesWorkplace = er.AtApprenticesWorkplace,
                    NumberOfApprentices = er.NumberOfApprentices,
                    Locations = er.EmployerRequestRegions.Select(requestRegion => requestRegion.Region.SubregionName).ToList()
                })
                .OrderByDescending(x => x.DateOfRequest)
                .ToListAsync();

            return result;
        }

        public async Task<List<EmployerRequestReviewModel>> GetForProviderResponse(Guid providerResponseId)
        {
            var result = await Entities
                .Include(er => er.ProviderResponseEmployerRequests)
                .ThenInclude(prer => prer.ProviderResponse)
                .Where(er => er.Standard != null && er.ProviderResponseEmployerRequests.Any(x => x.ProviderResponseId == providerResponseId))
                .Select(er => new EmployerRequestReviewModel
                {
                    EmployerRequestId = er.Id,
                    StandardReference = er.StandardReference,
                    StandardTitle = er.Standard.StandardTitle,
                    StandardLevel = er.Standard.StandardLevel,
                    DateOfRequest = er.RequestedAt,
                    DayRelease = er.DayRelease,
                    BlockRelease = er.BlockRelease,
                    AtApprenticesWorkplace = er.AtApprenticesWorkplace,
                    NumberOfApprentices = er.NumberOfApprentices,
                    Locations = er.EmployerRequestRegions.Select(requestRegion => requestRegion.Region.SubregionName).ToList(),
                })
                .OrderByDescending(x => x.DateOfRequest)
                .ToListAsync();

            return result;
        }
        public async Task ExpireEmployerRequests(int expiryAfterMonths) 
        {
            var dateTimeNow = DateTime.UtcNow;
            var expiryRequestsRequestedBeforeDate = dateTimeNow.AddMonths(-expiryAfterMonths);

            var result = await Entities
                .Where(er => er.RequestStatus == Models.Enums.RequestStatus.Active
                    && er.RequestedAt < expiryRequestsRequestedBeforeDate)
                .ToListAsync();

            result.ForEach(er => 
            {
                er.RequestStatus = Models.Enums.RequestStatus.Expired;
                er.ExpiredAt = dateTimeNow;
            });    
        }

        public async Task<List<EmployerRequestForResponseNotification>> GetForResponseNotification()
        { 
            var result = await Entities
                .Include(er => er.ProviderResponseEmployerRequests)
                .Where(er =>
                    er.Standard != null &&
                    er.RequestStatus == Models.Enums.RequestStatus.Active &&
                    er.ProviderResponseEmployerRequests.Any(pre => pre.ProviderResponseId.HasValue && !pre.AcknowledgedBy.HasValue)
                ) 
                .Select(er => new 
                {
                    StandardTitle = er.Standard.StandardTitle,
                    StandardLevel = er.Standard.StandardLevel,
                    AccountId = er.AccountId,
                    RequestedBy = er.RequestedBy,
                })
                .GroupBy(er => new { er.AccountId, er.RequestedBy })
                .Select(g => new EmployerRequestForResponseNotification
                {
                    AccountId = g.Key.AccountId,
                    RequestedBy = g.Key.RequestedBy,
                    Standard = g.Select(x => new StandardDetails
                    {
                        StandardTitle = x.StandardTitle,
                        StandardLevel = x.StandardLevel
                    }).ToList()
                })
                .ToListAsync();

            return result;
        }
    }
}
