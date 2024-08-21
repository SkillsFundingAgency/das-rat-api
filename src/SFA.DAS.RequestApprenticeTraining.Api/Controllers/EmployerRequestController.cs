﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Api.Extensions;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerAggregatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderAggregatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class EmployerRequestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployerRequestController> _logger;

        public EmployerRequestController(IMediator mediator, ILogger<EmployerRequestController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployerRequest([FromBody] CreateEmployerRequestCommand request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error creating employer request.");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating employer request.");
                return BadRequest();
            }
        }

        [HttpPut("{employerRequestId:guid}/acknowledge-responses")]
        public async Task<IActionResult> AcknowledgeProviderResponses(Guid employerRequestId, [FromQuery] Guid acknowledgedBy)
        {
            try
            {
                await _mediator.Send(new AcknowledgeProviderResponsesCommand(employerRequestId, acknowledgedBy));
                return Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error updating employer request to acknowledge responses.");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating employer request to acknowledge responses.");
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

        [HttpGet("account/{accountId}/aggregated")]
        public async Task<IActionResult> GetEmployerAggregatedEmployerRequests(long accountId)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerAggregatedEmployerRequestsQuery { AccountId = accountId });
                return Ok(result.EmployerAggregatedEmployerRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve aggregated employer requests for Employer: {accountId}");
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

        [HttpPost("provider/responses")]
        public async Task<IActionResult> CreateProviderResponses(CreateProviderResponseEmployerRequestsCommand request)
        {
            try
            {
                await _mediator.Send(request);
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
    }
}
