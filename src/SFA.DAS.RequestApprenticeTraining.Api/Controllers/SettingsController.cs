using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSettings;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class SettingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(IMediator mediator, ILogger<SettingsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetSettings()
        {
            try
            {
                var result = await _mediator.Send(new GetSettingsQuery());
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve settings");
                return BadRequest();
            }
        }
    }
}
