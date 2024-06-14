using Microsoft.EntityFrameworkCore;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Interfaces
{
    public interface IEmployerRequestRegionEntityContext : IEntityContext<EmployerRequestRegion>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public async Task<EmployerRequestRegion> GetForEmployerRequestId(Guid employerRequestId)
            => await Entities
                .FirstOrDefaultAsync(er => er.EmployerRequestId == employerRequestId);
    }
}
