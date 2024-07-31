using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenRequestingSelectEmployerRequests
    {
        [Test, AutoMoqData]
        public async Task And_SelectEmployerRequests_IsFound_ThenSelectEmployerRequestsAreReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var standard1Reference = "ST0001";
            var standard1Title = "Standard 1";

            var standard2Reference = "ST0002";
            var standard2Title = "Standard 2";

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle= standard1Title, StandardLevel = 1, StandardSector = "Sector 1"});
            context.Add(new Standard { StandardReference = standard2Reference, StandardTitle = standard2Title, StandardLevel = 2, StandardSector = "Sector 2" });
            context.Add(new EmployerRequest { Id = new Guid(), AccountId = 1, StandardReference= standard1Reference, NumberOfApprentices = 2, AtApprenticesWorkplace = false, BlockRelease = true, DayRelease = false, OriginalLocation ="Swansea (Original)", SingleLocation ="Swansea (Single)", RequestStatus = Domain.Models.Enums.RequestStatus.Active});
            context.Add(new EmployerRequest { Id = new Guid(), AccountId = 2, StandardReference = standard1Reference, NumberOfApprentices = 4, AtApprenticesWorkplace = true, BlockRelease = false, DayRelease = true, OriginalLocation = "Hull (Original)", SingleLocation = "Hull (Single)", RequestStatus = Domain.Models.Enums.RequestStatus.Active });
            context.Add(new EmployerRequest { Id = new Guid(), AccountId = 3, StandardReference = standard1Reference, NumberOfApprentices = 6, AtApprenticesWorkplace = false, BlockRelease = true, DayRelease = true, OriginalLocation = "London (Original)", SingleLocation = "London (Single)", RequestStatus = Domain.Models.Enums.RequestStatus.Active });
            context.Add(new EmployerRequest { Id = new Guid(), AccountId = 4, StandardReference = standard2Reference, NumberOfApprentices = 1, AtApprenticesWorkplace = true, BlockRelease = false, DayRelease = true, OriginalLocation = "Coventry (Original)", SingleLocation = "Coventry (Single)", RequestStatus = Domain.Models.Enums.RequestStatus.Active });

            context.SaveChanges();

            var query = new GetSelectEmployerRequestsQuery(123456789, "ST0001");
            var handler = new GetSelectEmployerRequestsQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.SelectEmployerRequests.Should().HaveCount(3);

        }

        [Test, AutoMoqData]
        public async Task And_SelectEmployerRequests_NoneFound_ThenEmptySelectEmployerRequestsAreReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var query = new GetSelectEmployerRequestsQuery(123456789, "ST0001");
            var handler = new GetSelectEmployerRequestsQueryHandler(context);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.SelectEmployerRequests.Should().BeEmpty();
        }
    }
}
