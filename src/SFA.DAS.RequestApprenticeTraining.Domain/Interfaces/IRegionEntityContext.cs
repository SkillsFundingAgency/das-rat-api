using Microsoft.EntityFrameworkCore;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Interfaces
{
    public interface IRegionEntityContext : IEntityContext<Region>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public async Task<List<Region>> GetAll()
            => await Entities.ToListAsync();
        
        Task<Region> FindClosestRegion(double latitude, double longitude);
    }
}
