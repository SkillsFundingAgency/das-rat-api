using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateProviderResponseEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.SubmitProviderResponse;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderAggregatedEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Api.Controllers
{
    [ApiController]
    [Route("api/providers/")]
    public class ProvidersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProvidersController> _logger;

        public ProvidersController(IMediator mediator, ILogger<ProvidersController> logger) 
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpGet("{ukprn}/employer-requests/aggregated")]
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

        [HttpGet("{ukprn}/employer-requests/{standardReference}/select")]
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

        [HttpPost("{ukprn}/employer-requests/acknowledge")]
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

        [HttpPost("{ukprn}/responses")]
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
    }
}
