using MediatR;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CreateEmployerRequest
{
    public class CreateEmployerRequestCommand : IRequest<CreateEmployerRequestCommandResponse>
    {
        public int RequestTypeId { get; set; }
    }
}