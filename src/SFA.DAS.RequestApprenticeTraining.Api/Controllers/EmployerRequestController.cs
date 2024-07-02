﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetAggregatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using System;
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

        public EmployerRequestController(IMediator mediator, ILogger<EmployerRequestController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEmployerRequestCommand request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving employer request to database.");
                return BadRequest();
            }
        }

        [HttpGet("{employerRequestId}")]
        public async Task<IActionResult> GetEmployerRequest(Guid employerRequestId)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerRequestQuery { EmployerRequestId = employerRequestId });
                return Ok(result.EmployerRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve employer request for Id: {employerRequestId}");
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
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve employer requests for AccountId: {accountId}");
                return BadRequest();
            }
        }

        [HttpGet("aggregated-employer-requests")]
        public async Task<IActionResult> GetAggregatedEmployerRequests()
        {
            try
            {
                var result = await _mediator.Send(new GetAggregatedEmployerRequestsQuery());
                return Ok(result.AggregatedEmployerRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve aggregated employer requests");
                return BadRequest();
            }
        }

        [HttpGet("select-employer-requests")]
        public async Task<IActionResult> GetSelectEmployerRequests([FromQuery]string standardReference, [FromQuery]string ukprn)
        {
            try
            {
                var result = await _mediator.Send(
                    new GetSelectEmployerRequestsQuery() { StandardReference = standardReference, Ukprn = ukprn });

                return Ok(result.SelectEmployerRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve select employer requests");
                return BadRequest();
            }
        }
    }
}
