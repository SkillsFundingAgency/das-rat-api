using Microsoft.EntityFrameworkCore;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Interfaces
{
    public interface IEmployerRequestRegionEntityContext : IEntityContext<EmployerRequestRegion>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public async Task<List<EmployerRequestRegion>> GetForEmployerRequestId(Guid employerRequestId)
            => await Entities
                .Where(er => er.EmployerRequestId == employerRequestId).ToListAsync();
    }
}
