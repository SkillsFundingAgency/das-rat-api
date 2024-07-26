using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Domain.Interfaces
{
    public interface IProviderResponseEmployerRequestStatusEntityContext : IEntityContext<ProviderResponseEmployerRequestStatus>
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
