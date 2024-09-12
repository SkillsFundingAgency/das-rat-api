using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenRequestingSelectEmployerRequests
    {
        private Mock<IOptions<ApplicationSettings>> _mockOptions;
        private DateTime _insideAllowedDateRangeForContactedRequests;
        private DateTime _outsideAllowedDateRangeForContactedRequests;

        [SetUp]
        public void SetUp()
        {
            _mockOptions = new Mock<IOptions<ApplicationSettings>>();

            var config = new ApplicationSettings
            {
                ProviderRemovedAfterRequestedMonths = 12,
            };
            _mockOptions.Setup(o => o.Value).Returns(config);

            _insideAllowedDateRangeForContactedRequests = DateTime.Now.AddMonths(-config.ProviderRemovedAfterRequestedMonths).AddDays(1);
            _outsideAllowedDateRangeForContactedRequests = DateTime.Now.AddMonths(-config.ProviderRemovedAfterRequestedMonths).AddDays(-1);
        }

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
            context.Add(new EmployerRequest { Id = new Guid(), AccountId = 1, StandardReference= standard1Reference, NumberOfApprentices = 2, AtApprenticesWorkplace = false, BlockRelease = true, DayRelease = false, OriginalLocation ="Swansea (Original)", SingleLocation ="Swansea (Single)", RequestStatus = Domain.Models.Enums.RequestStatus.Active, RequestedAt = _insideAllowedDateRangeForContactedRequests});
            context.Add(new EmployerRequest { Id = new Guid(), AccountId = 2, StandardReference = standard1Reference, NumberOfApprentices = 4, AtApprenticesWorkplace = true, BlockRelease = false, DayRelease = true, OriginalLocation = "Hull (Original)", SingleLocation = "Hull (Single)", RequestStatus = Domain.Models.Enums.RequestStatus.Active, RequestedAt = _insideAllowedDateRangeForContactedRequests });
            context.Add(new EmployerRequest { Id = new Guid(), AccountId = 3, StandardReference = standard1Reference, NumberOfApprentices = 6, AtApprenticesWorkplace = false, BlockRelease = true, DayRelease = true, OriginalLocation = "London (Original)", SingleLocation = "London (Single)", RequestStatus = Domain.Models.Enums.RequestStatus.Active, RequestedAt = _insideAllowedDateRangeForContactedRequests });
            context.Add(new EmployerRequest { Id = new Guid(), AccountId = 4, StandardReference = standard2Reference, NumberOfApprentices = 1, AtApprenticesWorkplace = true, BlockRelease = false, DayRelease = true, OriginalLocation = "Coventry (Original)", SingleLocation = "Coventry (Single)", RequestStatus = Domain.Models.Enums.RequestStatus.Active, RequestedAt = _insideAllowedDateRangeForContactedRequests });

            context.SaveChanges();

            var query = new GetSelectEmployerRequestsQuery(123456789, standard1Reference);
            var handler = new GetSelectEmployerRequestsQueryHandler(context, _mockOptions.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.SelectEmployerRequests.Should().HaveCount(3);
            result.SelectEmployerRequests.Should().OnlyContain(r => r.StandardReference == standard1Reference);

        }


        [Test, AutoMoqData]
        public async Task And_SelectEmployerRequests_IsContactedByThisProviderWithinAllowedDateRangeTrue_ThenSelectEmployerRequestsAreReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var ukprn = 789456;
            var standard1Reference = "ST0001";
            var standard1Title = "Standard 1";

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            var employerRequest1 = context.Add(new EmployerRequest { Id = Guid.NewGuid(), AccountId = 1, RequestedAt = _insideAllowedDateRangeForContactedRequests, StandardReference = standard1Reference, NumberOfApprentices = 1, AtApprenticesWorkplace = false, BlockRelease = true, DayRelease = false, RequestStatus = Domain.Models.Enums.RequestStatus.Active });
            
            var providerResponse = context.Add(new ProviderResponse { Id = Guid.NewGuid(), RespondedAt = DateTime.Now.AddMonths(-1), Email = "current@provider.com", PhoneNumber = "123456789", Website = "www.current.com" });
            context.Add(new ProviderResponseEmployerRequest { ProviderResponseId = providerResponse.Entity.Id, EmployerRequestId = employerRequest1.Entity.Id, Ukprn = ukprn });
            
            context.SaveChanges();

            var query = new GetSelectEmployerRequestsQuery(ukprn, standard1Reference);
            var handler = new GetSelectEmployerRequestsQueryHandler(context, _mockOptions.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.SelectEmployerRequests.Should().HaveCount(1);

            result.SelectEmployerRequests.Any(r => r.EmployerRequestId == employerRequest1.Entity.Id && r.IsContacted && r.DateContacted == providerResponse.Entity.RespondedAt).Should().BeTrue();
        }

        [Test, AutoMoqData]
        public async Task And_SelectEmployerRequests_IsContactedByOtherProvider_ThenSelectEmployerRequestsAreReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var ukprn = 789456;
            var otherUkprn = 344545;
            var standard1Reference = "ST0001";
            var standard1Title = "Standard 1";

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            var employerRequest1 = context.Add(new EmployerRequest { Id = Guid.NewGuid(), AccountId = 1, RequestedAt = _insideAllowedDateRangeForContactedRequests, StandardReference = standard1Reference, NumberOfApprentices = 1, AtApprenticesWorkplace = false, BlockRelease = true, DayRelease = false, RequestStatus = Domain.Models.Enums.RequestStatus.Active });
            var employerRequest2 = context.Add(new EmployerRequest { Id = Guid.NewGuid(), AccountId = 2, RequestedAt = _insideAllowedDateRangeForContactedRequests, StandardReference = standard1Reference, NumberOfApprentices = 2, AtApprenticesWorkplace = true, BlockRelease = false, DayRelease = true, RequestStatus = Domain.Models.Enums.RequestStatus.Active });
            
            var providerResponse = context.Add(new ProviderResponse { Id = Guid.NewGuid(), RespondedAt = DateTime.Now.AddMonths(-1), Email = "current@provider.com", PhoneNumber = "123456789", Website = "www.current.com" });
            context.Add(new ProviderResponseEmployerRequest { ProviderResponseId = providerResponse.Entity.Id, EmployerRequestId = employerRequest1.Entity.Id, Ukprn = otherUkprn });

            var providerResponseExpired = context.Add(new ProviderResponse { Id = Guid.NewGuid(), RespondedAt = DateTime.Now.AddMonths(-13), Email = "expired@provider.com", PhoneNumber = "987654321", Website = "www.expired.com" });
            context.Add(new ProviderResponseEmployerRequest { ProviderResponseId = providerResponseExpired.Entity.Id, EmployerRequestId = employerRequest2.Entity.Id, Ukprn = otherUkprn });

            context.SaveChanges();

            var query = new GetSelectEmployerRequestsQuery(ukprn, standard1Reference);
            var handler = new GetSelectEmployerRequestsQueryHandler(context, _mockOptions.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.SelectEmployerRequests.Should().HaveCount(2);

            result.SelectEmployerRequests.Any(r => r.EmployerRequestId == employerRequest1.Entity.Id && !r.IsContacted && !r.DateContacted.HasValue).Should().BeTrue();
            result.SelectEmployerRequests.Any(r => r.EmployerRequestId == employerRequest2.Entity.Id && !r.IsContacted && !r.DateContacted.HasValue).Should().BeTrue();

        }

        [Test, AutoMoqData]
        public async Task And_SelectEmployerRequests_IsContactedTrue_RequestedOutsideAllowedDateRangeTrue_ThenSelectEmployerRequestsAreNotReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var ukprn = 789456;
            var standard1Reference = "ST0001";
            var standard1Title = "Standard 1";

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            var employerRequest1 = context.Add(new EmployerRequest { Id = Guid.NewGuid(), AccountId = 1, RequestedAt = _outsideAllowedDateRangeForContactedRequests, StandardReference = standard1Reference, NumberOfApprentices = 1, AtApprenticesWorkplace = false, BlockRelease = true, DayRelease = false, RequestStatus = Domain.Models.Enums.RequestStatus.Active });

            var providerResponse = context.Add(new ProviderResponse { Id = Guid.NewGuid(), RespondedAt = DateTime.Now.AddMonths(-1), Email = "current@provider.com", PhoneNumber = "123456789", Website = "www.current.com" });
            context.Add(new ProviderResponseEmployerRequest { ProviderResponseId = providerResponse.Entity.Id, EmployerRequestId = employerRequest1.Entity.Id, Ukprn = ukprn });

            context.SaveChanges();

            var query = new GetSelectEmployerRequestsQuery(ukprn, standard1Reference);
            var handler = new GetSelectEmployerRequestsQueryHandler(context, _mockOptions.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.SelectEmployerRequests.Should().BeEmpty();
         }

        [Test, AutoMoqData]
        public async Task And_SelectEmployerRequests_NoneFound_ThenEmptySelectEmployerRequestsAreReturned(
            [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var query = new GetSelectEmployerRequestsQuery(123456789, "ST0001");
            var handler = new GetSelectEmployerRequestsQueryHandler(context, _mockOptions.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.SelectEmployerRequests.Should().BeEmpty();
        }
    }
}
