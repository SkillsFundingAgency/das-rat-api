using MediatR;
using SFA.DAS.RequestApprenticeTraining.Application.Models;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.CancelEmployerRequest
{
    public class CancelEmployerRequestCommand : IRequest
    {
        public Guid EmployerRequestId { get; set; }
        public Guid CancelledBy { get; set;  }

        public static implicit operator CancelEmployerRequestCommand(CancelEmployerRequestRequest source)
        {
            return new CancelEmployerRequestCommand
            {
                CancelledBy = source.CancelledBy
            };
        }
    }
}