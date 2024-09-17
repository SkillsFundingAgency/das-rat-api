using MediatR;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System.Collections.Generic;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.RefreshStandards

{
    public class RefreshStandardsCommand : IRequest<Unit>
    {
        public List<RefreshStandardsParameter> Standards { get; set; }
    }
}