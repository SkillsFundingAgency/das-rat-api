using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Api.Extensions;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.AcknowledgeProviderResponses;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Models;
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
                _logger.LogError(ex, "Validation error attempting to retrieve employer request for {AccountId} and {StandardReference}", accountId, standardReference.SanitizeLogData());
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve employer request for {AccountId} and {StandardReference}", accountId, standardReference.SanitizeLogData());
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
                _logger.LogError(ex, "Validation error attempting to retrieve employer requests for {AccountId}", accountId);
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attempting to retrieve employer requests for {AccountId}", accountId);
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
                _logger.LogError(ex, "Validation attempting to create provider responses");
                return BadRequest(new { errors = ex.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to create provider responses");
                return BadRequest();
            }
        }
    }
}
