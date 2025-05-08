using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Api.TaskQueue;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CancelEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.ExpireEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Extensions;
using SFA.DAS.RequestApprenticeTraining.Application.Models;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsForResponseNotification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("api/employer-requests/")]
    public class EmployerRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployerRequestsController> _logger;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        public EmployerRequestsController(IMediator mediator, ILogger<EmployerRequestsController> logger, IBackgroundTaskQueue taskQueue)
        {
            _mediator = mediator;
            _logger = logger;
            _backgroundTaskQueue = taskQueue;
        }

        [HttpGet("{employerRequestId}")]
        public async Task<IActionResult> GetEmployerRequest(Guid employerRequestId)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerRequestQuery { EmployerRequestId = employerRequestId });
                return Ok(result.EmployerRequest);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to retrieve employer request for {EmployerRequestId}", employerRequestId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve employer request for {EmployerRequestId}", employerRequestId);
                return BadRequest();
            }
        }

        [HttpPut("{employerRequestId}/responses/acknowledge")]
        public async Task<IActionResult> AcknowledgeProviderResponses(Guid employerRequestId, [FromBody] AcknowledgeProviderResponsesRequest request)
        {
            try
            {
                var command = (AcknowledgeProviderResponsesCommand)request;
                command.EmployerRequestId = employerRequestId;

                await _mediator.Send(command);
                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to acknowledge responses for employer request {EmployerRequestId}", employerRequestId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to acknowledge responses for employer request {EmployerRequestId}", employerRequestId);
                return BadRequest();
            }
        }

        [HttpPut("{employerRequestId}/cancel")]
        public async Task<IActionResult> CancelEmployerRequest([FromRoute] Guid employerRequestId, [FromBody] CancelEmployerRequestRequest request)
        {
            try
            {
                var command = (CancelEmployerRequestCommand)request;
                command.EmployerRequestId = employerRequestId;

                await _mediator.Send(command);
                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to cancel employer request {EmployerRequestId}", employerRequestId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to cancel employer request {EmployerRequestId}", employerRequestId);
                return BadRequest();
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetEmployerRequestsByIds([FromQuery] List<Guid> employerRequestIds)
        {
            try
            {
                _logger.LogInformation("GetEmployerRequestsByIds call initiated");
                var result = await _mediator.Send(
                    new GetEmployerRequestsByIdsQuery(employerRequestIds));

                return Ok(result.EmployerRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve select employer requests");
                return BadRequest();
            }
        }

        [HttpPut("expire")]
        public IActionResult ExpireEmployerRequests()
        {
            var requestName = "expire employer requests";
            try
            {
                _backgroundTaskQueue.QueueBackgroundRequest(
                    new ExpireEmployerRequestsCommand(), requestName, (response, duration, log) =>
                    {
                        log.LogInformation("Completed request to {RequestName}: Request completed in {Duration}", requestName, duration.ToReadableString());
                    });

                _logger.LogInformation("Queued request to {RequestName}", requestName);

                return Accepted();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to {RequestName}", requestName);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error {requestName}: {e.Message}");
            }
        }

        [HttpGet("response-notifications")]
        public async Task<IActionResult> GetEmployerRequestsForResponseNotifications()
        {
            try
            {
                var result = await _mediator.Send(
                    new GetEmployerRequestsForResponseNotificationQuery());

                return Ok(result.EmployerRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve employer requests for response notifications");
                return BadRequest();
            }
        }
    }
}
