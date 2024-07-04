using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetClosestRegion;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetRegions;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class RegionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(IMediator mediator, ILogger<RegionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetRegions()
        {
            try
            {
                var result = await _mediator.Send(new GetRegionsQuery());
                return Ok(result.Regions);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve regions");
                return BadRequest();
            }
        }

        [HttpGet("closest")]
        public async Task<IActionResult> GetClosestRegion([FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                var result = await _mediator.Send(new GetClosestRegionQuery { Latitude = latitude, Longitude = longitude});
                return Ok(result.Region);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve closest region");
                return BadRequest();
            }
        }
    }
}
