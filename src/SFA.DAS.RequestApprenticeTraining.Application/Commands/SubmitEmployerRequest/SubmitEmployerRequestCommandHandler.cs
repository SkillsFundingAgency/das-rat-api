﻿using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitEmployerRequest
{
    public class SubmitEmployerRequestCommandHandler : IRequestHandler<SubmitEmployerRequestCommand, SubmitEmployerRequestCommandResponse>
    {
        private readonly IEmployerRequestEntityContext _employerRequestEntityContext;
        private readonly IEmployerRequestRegionEntityContext _employerRequestRegionEntityContext;
        private readonly IRegionEntityContext _regionEntityContext;
        private readonly ILogger<SubmitEmployerRequestCommandHandler> _logger;

        public SubmitEmployerRequestCommandHandler(
            IEmployerRequestEntityContext employerRequestEntityContext,
            IEmployerRequestRegionEntityContext employerRequestRegionEntityContext,
            IRegionEntityContext regionEntityContext,
            ILogger<SubmitEmployerRequestCommandHandler> logger)
        {
            _employerRequestEntityContext = employerRequestEntityContext;
            _employerRequestRegionEntityContext = employerRequestRegionEntityContext;
            _regionEntityContext = regionEntityContext;
            _logger = logger;
        }

        public async Task<SubmitEmployerRequestCommandResponse> Handle(SubmitEmployerRequestCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Creating employer request");

            var regions = new List<Region>();
            if (string.IsNullOrEmpty(request.SameLocation) || request.SameLocation == "Yes")
            {
                var closestRegion = await _regionEntityContext.FindClosestRegion(request.SingleLocationLatitude, request.SingleLocationLongitude);
                if (closestRegion != null)
                {
                    regions.Add(closestRegion);
                    _logger.LogDebug("Using matched single location to closest region {SubRegionName}", closestRegion.SubregionName);
                }
            }
            else
            {
                foreach (string location in request.MultipleLocations)
                {
                    var region = await _regionEntityContext.Get(int.Parse(location));
                    regions.Add(region);
                }
                
                _logger.LogDebug($"Using multiple selected regions");
            }

            var employerRequest = new EmployerRequest()
            {
                OriginalLocation = request.OriginalLocation,
                RequestType = request.RequestType,
                AccountId = request.AccountId,
                StandardReference = request.StandardReference,
                NumberOfApprentices = request.NumberOfApprentices,
                SameLocation = request.SameLocation,
                SingleLocation = request.SingleLocation,
                AtApprenticesWorkplace = request.AtApprenticesWorkplace,
                DayRelease = request.DayRelease,
                BlockRelease = request.BlockRelease,
                RequestedAt = DateTime.UtcNow,
                RequestedBy = request.RequestedBy,
                RequestStatus = RequestStatus.Active,
                ModifiedBy = request.ModifiedBy
            };

            _employerRequestEntityContext.Add(employerRequest);
            await _employerRequestEntityContext.SaveChangesAsync();

            foreach(var region in regions)
            {
                _employerRequestRegionEntityContext.Add(new EmployerRequestRegion
                {
                    EmployerRequestId = employerRequest.Id,
                    RegionId = region.Id,
                    ModifiedBy = request.ModifiedBy
                });
            }

            await _employerRequestRegionEntityContext.SaveChangesAsync();

            _logger.LogDebug("Created employer request record with {EmployerRequestId}", employerRequest.Id);

            return new SubmitEmployerRequestCommandResponse() { EmployerRequestId = employerRequest.Id };
        }
    }
}