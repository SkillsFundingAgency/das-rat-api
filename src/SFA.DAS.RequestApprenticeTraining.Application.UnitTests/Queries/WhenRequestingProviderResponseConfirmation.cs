using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetProviderResponseConfirmation;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenRequestingProviderResponseConfirmation
    {
        [Test, AutoMoqData]
        public async Task And_ProviderResponse_IsFound_ThenProviderResponseConfirmationReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context,
            string email,
            string phone,
            string website,
            long ukprn
            )
        {
            // Arrange
            var standard1Reference = "ST0001";
            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = "S1 Title", StandardLevel = 1, StandardSector = "Sector 1" });
            
            var request1 = new EmployerRequest { AccountId = 1, StandardReference = standard1Reference, NumberOfApprentices = 2, AtApprenticesWorkplace = false, BlockRelease = true, DayRelease = false, OriginalLocation = "Swansea (Original)", SingleLocation = "Swansea (Single)", RequestStatus = Domain.Models.Enums.RequestStatus.Active };
            var request2 = new EmployerRequest { AccountId = 2, StandardReference = standard1Reference, NumberOfApprentices = 4, AtApprenticesWorkplace = true, BlockRelease = false, DayRelease = true, OriginalLocation = "Hull (Original)", SingleLocation = "Hull (Single)", RequestStatus = Domain.Models.Enums.RequestStatus.Active };
            context.Add(request1);
            context.Add(request2);
            context.SaveChanges();

            var providerResponse = new ProviderResponse { Email = email, PhoneNumber = phone, Website = website, RespondedAt = DateTime.UtcNow, };
            var providerResponseToRequest1 = new ProviderResponseEmployerRequest { EmployerRequest = request1, ProviderResponse = providerResponse, Ukprn = ukprn };
            var providerResponseToRequest2 = new ProviderResponseEmployerRequest { EmployerRequest = request2, ProviderResponse = providerResponse, Ukprn = ukprn };

            context.Add(providerResponse);
            context.Add(providerResponseToRequest1);
            context.Add(providerResponseToRequest2);
            context.SaveChanges();

            var query = new GetProviderResponseConfirmationQuery(providerResponse.Id);
            var handler = new GetProviderResponseConfirmationQueryHandler(context, context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(email);
            result.Phone.Should().Be(phone);
            result.Website.Should().Be(website);
            result.Ukprn.Should().Be(ukprn);
            result.EmployerRequests.Should().HaveCount(2);


        }

        [Test, AutoMoqData]
        public async Task And_ProviderResponse_NoneFound_ThenEmptyProviderResponseConfirmationReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context,
            Guid providerResponseId)
        {
            // Arrange
            var query = new GetProviderResponseConfirmationQuery(providerResponseId);
            var handler = new GetProviderResponseConfirmationQueryHandler(context, context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().BeNullOrEmpty();
            result.Phone.Should().BeNullOrEmpty();
            result.Website.Should().BeNullOrEmpty();
            result.Ukprn.Should().Be(0);
            result.EmployerRequests.Should().BeEmpty();
        }
    }
}
