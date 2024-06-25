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
                .GroupBy(er => new { er.StandardReference, er.Course.Title, er.Course.Level, er.Course.Sector })
                .Select(g => new AggregatedEmployerRequest
                {
                    CourseReference = g.Key.StandardReference,
                    NumberOfApprentices = g.Sum(x => x.NumberOfApprentices),
                    NumberOfEmployers = g.Count(),
                    CourseTitle = g.Key.Title,
                    Level = g.Key.Level,
                    Sector = g.Key.Sector
                })
                .OrderBy(x => x.CourseReference)
                .ToListAsync();

            return result;
        }



    }
}
