using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.ExpireEmployerRequests;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetEmployerRequestsByIds;
using SFA.DAS.RequestApprenticeTraining.Application.UnitTests;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RequestStatus = SFA.DAS.RequestApprenticeTraining.Domain.Models.Enums.RequestStatus;

namespace SFA.DAS.RequestApprenticeTraining.UnitTests.Application.Commands.ExpireEmployerRequests
{
    [TestFixture]
    public class ExpireEmployerRequestsCommandHandlerTests
    {
        private Mock<IEmployerRequestEntityContext> _employerRequestEntityContextMock;
        private Mock<ILogger<ExpireEmployerRequestsCommandHandler>> _loggerMock;
        private Mock<IOptions<ApplicationSettings>> _optionsMock;
        private ExpireEmployerRequestsCommandHandler _sut;
        private static int _expiryAfterMonths = 3;

        [SetUp]
        public void SetUp()
        {
            _employerRequestEntityContextMock = new Mock<IEmployerRequestEntityContext>();
            _loggerMock = new Mock<ILogger<ExpireEmployerRequestsCommandHandler>>();

            var config = new ApplicationSettings { ExpiryAfterMonths = _expiryAfterMonths };
            _optionsMock = new Mock<IOptions<ApplicationSettings>>();
            _optionsMock.Setup(o => o.Value).Returns(config);

            _sut = new ExpireEmployerRequestsCommandHandler(
                _employerRequestEntityContextMock.Object,
                _loggerMock.Object,
                _optionsMock.Object
                );
        }

        [Test]
        public async Task Handle_ShouldCallExpireEmployerRequests()
        {
            // Act
            var response = await _sut.Handle(new ExpireEmployerRequestsCommand(), CancellationToken.None);

            // Assert
            _employerRequestEntityContextMock.Verify(x => x.ExpireEmployerRequests(It.IsAny<int>()), Times.Once);
            _employerRequestEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, AutoMoqData]
        public async Task And_EmployerRequestsAreExpired(
           [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var standard1Reference = "ST0001";
            var standard1Title = "Standard 1";

            var standard2Reference = "ST0002";
            var standard2Title = "Standard 2";

            var guids = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            context.Add(new Standard { StandardReference = standard2Reference, StandardTitle = standard2Title, StandardLevel = 2, StandardSector = "Sector 2" });
            context.Add(new EmployerRequest { Id = guids[0], RequestStatus = RequestStatus.Active, RequestedAt = DateTime.UtcNow.AddMonths(-_expiryAfterMonths).AddMonths(-1), ExpiredAt = null });
            context.Add(new EmployerRequest { Id = guids[1], RequestStatus = RequestStatus.Active, RequestedAt = DateTime.UtcNow.AddMonths(-_expiryAfterMonths).AddHours(-1), ExpiredAt = null });
            context.Add(new EmployerRequest { Id = guids[2], RequestStatus = RequestStatus.Active, RequestedAt = DateTime.UtcNow.AddMonths(-_expiryAfterMonths).AddDays(-1), ExpiredAt = null });
            context.SaveChanges();

            var handler = new ExpireEmployerRequestsCommandHandler(context, _loggerMock.Object, _optionsMock.Object);

            // Act
            var result = await handler.Handle(new ExpireEmployerRequestsCommand(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            context.EmployerRequests.Where(x => x.RequestStatus == RequestStatus.Active).Should().HaveCount(0);
            context.EmployerRequests.Where(x => x.RequestStatus == RequestStatus.Expired).Should().HaveCount(3);

        }

        [Test, AutoMoqData]
        public async Task And_EmployerRequestsAreNotExpired(
           [Frozen(Matching.ImplementedInterfaces)] RequestApprenticeTrainingDataContext context)
        {
            // Arrange
            var standard1Reference = "ST0001";
            var standard1Title = "Standard 1";

            var standard2Reference = "ST0002";
            var standard2Title = "Standard 2";

            var guids = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            context.Add(new RequestType { Id = 1, Description = "Shortlist" });
            context.Add(new Standard { StandardReference = standard1Reference, StandardTitle = standard1Title, StandardLevel = 1, StandardSector = "Sector 1" });
            context.Add(new Standard { StandardReference = standard2Reference, StandardTitle = standard2Title, StandardLevel = 2, StandardSector = "Sector 2" });
            context.Add(new EmployerRequest { Id = guids[0], RequestStatus = RequestStatus.Active, RequestedAt = DateTime.UtcNow.AddMonths(-_expiryAfterMonths).AddMonths(1), ExpiredAt = null });
            context.Add(new EmployerRequest { Id = guids[1], RequestStatus = RequestStatus.Active, RequestedAt = DateTime.UtcNow.AddMonths(-_expiryAfterMonths).AddHours(1), ExpiredAt = null });
            context.Add(new EmployerRequest { Id = guids[2], RequestStatus = RequestStatus.Active, RequestedAt = DateTime.UtcNow.AddMonths(-_expiryAfterMonths).AddDays(1), ExpiredAt = null });
            context.Add(new EmployerRequest { Id = guids[3], RequestStatus = RequestStatus.Cancelled, RequestedAt = DateTime.UtcNow.AddMonths(-_expiryAfterMonths).AddDays(1), ExpiredAt = null });
            context.Add(new EmployerRequest { Id = guids[4], RequestStatus = RequestStatus.Cancelled, RequestedAt = DateTime.UtcNow.AddMonths(-_expiryAfterMonths).AddDays(-1), ExpiredAt = null });
            context.SaveChanges();

            var handler = new ExpireEmployerRequestsCommandHandler(context, _loggerMock.Object, _optionsMock.Object);

            // Act
            var result = await handler.Handle(new ExpireEmployerRequestsCommand(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            context.EmployerRequests.Where(x => x.RequestStatus == RequestStatus.Expired).Should().HaveCount(0);
            context.EmployerRequests.Where(x => x.RequestStatus == RequestStatus.Active).Should().HaveCount(3);
            context.EmployerRequests.Where(x => x.RequestStatus == RequestStatus.Cancelled).Should().HaveCount(2);

        }

    }
}
