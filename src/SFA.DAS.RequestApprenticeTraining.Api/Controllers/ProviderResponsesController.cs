using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("api/provider-responses/")]
    public class ProviderResponsesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderResponsesController> _logger;

        public ProviderResponsesController(IMediator mediator, ILogger<ProviderResponsesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{providerResponseId}/confirmation")]
        public async Task<IActionResult> GetProviderResponseConfirmation(Guid providerResponseId)
        {
            try
            {
                var result = await _mediator.Send(
                    new GetProviderResponseConfirmationQuery(providerResponseId));

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve provider response confirmation");
                return BadRequest();
            }
        }
    }
}
