using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.RefreshStandards;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class StandardsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StandardsController> _logger;

        public StandardsController(IMediator mediator, ILogger<StandardsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPut("refresh")]
        public async Task<IActionResult> RefreshStandards([FromBody] RefreshStandardsParameters parameters)
        {
            try
            {
                await _mediator.Send(new RefreshStandardsCommand
                {
                    Standards = parameters.Standards,
                });
                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error refreshing standards.");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error refreshing standards.");
                return BadRequest();
            }
        }


    }
}
