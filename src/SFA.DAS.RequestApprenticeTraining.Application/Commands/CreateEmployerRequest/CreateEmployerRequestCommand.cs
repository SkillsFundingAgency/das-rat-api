using MediatR;
using static SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateEmployerRequest
{
    public class CreateEmployerRequestCommand : IRequest<CreateEmployerRequestCommandResponse>
    {
        public RequestType RequestType { get; set; }
    }
}