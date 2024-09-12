﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Api.Extensions;
using SFA.DAS.RequestApprenticeTraining.Api.TaskQueue;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CancelEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.ExpireEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.RequestApprenticeTraining.Application.Extensions;
using SFA.DAS.RequestApprenticeTraining.Application.Models;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerAggregatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderAggregatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System;
using System.Collections.Generic;
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

        [HttpGet("account/{accountId}/standard/{standardReference}/active")]
        public async Task<IActionResult> GetActiveEmployerRequest(long accountId, string standardReference)
        {
            try
            {
                var result = await _mediator.Send(new GetActiveEmployerRequestQuery { AccountId = accountId, StandardReference = standardReference });
                return Ok(result.EmployerRequest);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to retrieve active employer request for {AccountId} and {StandardReference}", accountId, standardReference.SanitizeLogData());
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve active employer request for {AccountId} and {StandardReference}", accountId, standardReference.SanitizeLogData());
                return BadRequest();
            }
        }

        [HttpPut("{employerRequestId}/acknowledge-responses")]
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

        [HttpPut("{employerRequestId}/cancel-request")]
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

        [HttpPost("account/{accountId}/submit-request")]
        public async Task<IActionResult> SubmitEmployerRequest([FromRoute] long accountId, [FromBody] SubmitEmployerRequestRequest request)
        {
            try
            {
                var command = (SubmitEmployerRequestCommand)request;
                command.AccountId = accountId;

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error attempting to create employer request.");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to create employer request.");
                return BadRequest();
            }
        }

        [HttpGet("account/{accountId}/aggregated")]
        public async Task<IActionResult> GetEmployerAggregatedEmployerRequests(long accountId)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerAggregatedEmployerRequestsQuery { AccountId = accountId });
                return Ok(result.EmployerAggregatedEmployerRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve employer aggregated employer requests for {AccountId}", accountId);
                return BadRequest();
            }
        }

        [HttpGet("provider/{ukprn}/aggregated")]
        public async Task<IActionResult> GetProviderAggregatedEmployerRequests(long ukprn)
        {
            try
            {
                var result = await _mediator.Send(new GetProviderAggregatedEmployerRequestsQuery { Ukprn = ukprn });
                return Ok(result.ProviderAggregatedEmployerRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve provider aggregated employer requests for {Ukprn}", ukprn);
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
                _logger.LogError(e, "Error attempting to retrieve select employer requests for {Ukprn} and {StandardReference}", ukprn, standardReference);
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
                _logger.LogError(ex, "Validation attempting to create provider responses");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to create provider responses");
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
    }
}
