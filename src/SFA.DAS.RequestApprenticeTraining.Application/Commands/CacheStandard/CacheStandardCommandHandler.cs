using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;


namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CacheStandard
{
    public class CacheStandardCommandHandler : IRequestHandler<CacheStandardCommand, CacheStandardCommandResponse>
    {
        private readonly IStandardEntityContext _standardEntityContext;
        private readonly ILogger<CacheStandardCommandHandler> _logger;

        public CacheStandardCommandHandler(
            IStandardEntityContext standardEntityContext,

            ILogger<CacheStandardCommandHandler> logger)
        {
            _standardEntityContext = standardEntityContext;
            _logger = logger;
        }

        public async Task<CacheStandardCommandResponse> Handle(CacheStandardCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Caching standard {StandardReference}", request.StandardReference);

            var standard = await _standardEntityContext.Get(request.StandardReference);
            if (standard == null)
            {
                standard = await AddStandard(request);
            }
            else
            {
                standard = await UpdateStandard(standard, request);
            }

            return new CacheStandardCommandResponse
            {
                Standard = (Domain.Models.Standard)standard,
            };
        }

        private async Task<Standard> AddStandard(CacheStandardCommand request)
        {
            var standard = new Standard
            {
                StandardReference = request.StandardReference,
                StandardLevel = request.StandardLevel,
                StandardSector = request.StandardSector,
                StandardTitle = request.StandardTitle,
            };

            _standardEntityContext.Add(standard);
            await _standardEntityContext.SaveChangesAsync();

            _logger.LogDebug("Created standard {StandardReference}", request.StandardReference);
            return standard;
        }

        private async Task<Standard> UpdateStandard(Standard standard, CacheStandardCommand request)
        {
            if (standard.StandardTitle != request.StandardTitle ||
                standard.StandardLevel != request.StandardLevel ||
                standard.StandardSector != request.StandardSector)
            {
                standard.StandardTitle = request.StandardTitle;
                standard.StandardLevel = request.StandardLevel;
                standard.StandardSector = request.StandardSector;

                await _standardEntityContext.SaveChangesAsync();

                _logger.LogDebug("Updated standard {StandardReference}", request.StandardReference);
            }
            return standard;
        }

    }
}