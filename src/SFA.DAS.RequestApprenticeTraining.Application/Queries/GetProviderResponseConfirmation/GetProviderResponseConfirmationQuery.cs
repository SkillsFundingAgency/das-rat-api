using MediatR;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation
{
    public class GetProviderResponseConfirmationQuery : IRequest<GetProviderResponseConfirmationQueryResult>
    {
        public Guid ProviderResponseId { get; set; }

        public GetProviderResponseConfirmationQuery(Guid providerResponseId) 
        {
            ProviderResponseId = providerResponseId;
        }
    }
}
