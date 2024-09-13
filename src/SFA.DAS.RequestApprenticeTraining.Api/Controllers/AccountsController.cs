using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Api.Extensions;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Models;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerAggregatedEmployerRequests;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("api/accounts/")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IMediator mediator, ILogger<AccountsController> logger) 
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{accountId}/standard/{standardReference}/employer-request/active")]
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

        [HttpPost("{accountId}/employer-requests")]
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

        [HttpGet("{accountId}/employer-requests/aggregated")]
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
    }
}
