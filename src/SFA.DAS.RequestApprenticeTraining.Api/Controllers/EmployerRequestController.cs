using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Api.Extensions;
using SFA.DAS.RequestApprenticeTraining.Api.TaskQueue;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.ExpireEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.RequestApprenticeTraining.Application.Extensions;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class EmployerRequestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployerRequestController> _logger;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        public EmployerRequestController(IMediator mediator, ILogger<EmployerRequestController> logger, IBackgroundTaskQueue taskQueue) 
        {
            _mediator = mediator;
            _logger = logger;
            _backgroundTaskQueue = taskQueue;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEmployerRequestCommand request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error saving employer request to database.");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving employer request to database.");
                return BadRequest();
            }
        }

        [HttpGet("{employerRequestId:guid}")]
        public async Task<IActionResult> GetEmployerRequest(Guid employerRequestId)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerRequestQuery { EmployerRequestId = employerRequestId });
                return Ok(result.EmployerRequest);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, $"Validation error attempting to retrieve employer request for Id: {employerRequestId}");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve employer request for Id: {employerRequestId}");
                return BadRequest();
            }
        }

        [HttpGet("account/{accountId}/standard/{standardReference}")]
        public async Task<IActionResult> GetEmployerRequest(long accountId, string standardReference)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerRequestQuery { AccountId = accountId, StandardReference = standardReference });
                return Ok(result.EmployerRequest);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, $"Validation error attempting to retrieve employer request for AccountId: {accountId} and StandardReference: {standardReference.SanitizeLogData()}");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve employer request for Ukprn: {accountId} and StandardReference: {standardReference.SanitizeLogData}");
                return BadRequest();
            }
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetEmployerRequests(long accountId)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerRequestsQuery { AccountId = accountId });
                return Ok(result.EmployerRequests);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, $"Validation error attempting to retrieve employer requests for Ukprn: {accountId}");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve employer requests for Ukprn: {accountId}");
                return BadRequest();
            }
        }

        [HttpGet("provider/{ukprn}/aggregated")]
        public async Task<IActionResult> GetAggregatedEmployerRequests(long ukprn)
        {
            try
            {
                var result = await _mediator.Send(new GetAggregatedEmployerRequestsQuery { Ukprn = ukprn });
                return Ok(result.AggregatedEmployerRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve aggregated employer requests for Provider: {ukprn}");
                return BadRequest();
            }
        }

        [HttpGet("provider/{ukprn}/selectrequests/{standardReference}")]
        public async Task<IActionResult> GetSelectEmployerRequests(long ukprn, string standardReference)
        {
            try
            {
                var result = await _mediator.Send(
                    new GetSelectEmployerRequestsQuery(ukprn, standardReference));

                return Ok(result.SelectEmployerRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve select employer requests");
                return BadRequest();
            }
        }

        [HttpPost("provider/{ukprn}/acknowledge-requests")]
        public async Task<IActionResult> AcknowledgeEmployerRequests(long ukprn, AcknowledgeEmployerRequestsParameters parameters)
        {
            try
            {
                await _mediator.Send(new AcknowledgeEmployerRequestsCommand
                { 
                    Ukprn = ukprn,
                    EmployerRequestIds = parameters.EmployerRequestIds,
                });
                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error saving provider response to database.");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving provider response to database.");
                return BadRequest();
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetEmployerRequestsByIds([FromQuery]List<Guid> employerRequestIds)
        {
            try
            {
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

        [HttpPost("provider/{ukprn}/submit-response")]
        public async Task<IActionResult> SubmitProviderResponse(long ukprn, SubmitProviderResponseParameters parameters)
        {
            try
            {
                var response = await _mediator.Send(new SubmitProviderResponseCommand
                {
                    Ukprn = ukprn,
                    ContactName = parameters.ContactName,
                    RespondedBy = parameters.RespondedBy, 
                    Email = parameters.Email,
                    EmployerRequestIds = parameters.EmployerRequestIds,
                    Phone = parameters.Phone,
                    Website = parameters.Website,
                });
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error submitting provider response to database.");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving provider response to database.");
                return BadRequest();
            }
        }

        [HttpGet("providerResponse/{providerResponseId}/confirmation")]
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

        [HttpPost("expire-requests")]
        public IActionResult ExpireEmployerRequests()
        {
            var requestName = "expire employer requests";
            try
            {
                _backgroundTaskQueue.QueueBackgroundRequest(
                    new ExpireEmployerRequestsCommand(), requestName, (response, duration, log) =>
                    {
                        var result = response as ExpireEmployerRequestsCommand;
                        log.LogInformation($"Completed request to {requestName}: Request completed in {duration.ToReadableString()}");
                    });

                _logger.LogInformation($"Queued request to {requestName}");

                return Accepted();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to {requestName}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error {requestName}: {e.Message}");
            }
        }
    }
}
