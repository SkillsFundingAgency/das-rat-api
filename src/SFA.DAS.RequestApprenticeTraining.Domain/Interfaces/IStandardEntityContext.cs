using Microsoft.EntityFrameworkCore;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Interfaces
{
    public interface IStandardEntityContext : IEntityContext<Standard>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public async Task<Standard> Get(string standardReference)
            => await Entities
                .FirstOrDefaultAsync(er => er.StandardReference == standardReference);

        public async Task<List<Standard>> GetAll()
            => await Entities.ToListAsync();

        public async Task AddRange(List<Standard> standards)    
            => await Entities.AddRangeAsync(standards);

    }
}
