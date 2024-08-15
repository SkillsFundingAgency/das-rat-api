using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSettings
{
    public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, GetSettingsQueryResult>
    {
        private readonly ApplicationSettings _applicationSettings;

        public GetSettingsQueryHandler(IOptions<ApplicationSettings> options)
        {
            _applicationSettings = options.Value;
        }

        public async Task<GetSettingsQueryResult> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new GetSettingsQueryResult
            {
                ExpiryAfterMonths = _applicationSettings.ExpiryAfterMonths,
                RemovedAfterExpiryContactedMonths = _applicationSettings.RemovedAfterExpiryContactedMonths,
                RemovedAfterExpiryNotContactedMonths = _applicationSettings.RemovedAfterExpiryNotContactedMonths
            });
        }
    }
}
