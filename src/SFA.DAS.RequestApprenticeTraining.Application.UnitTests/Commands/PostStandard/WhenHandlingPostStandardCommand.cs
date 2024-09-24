using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Commands.PostStandard;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Commands.PostStandard
{
    [TestFixture]
    public class WhenHandlingPostStandardCommand
    {
        private Mock<IStandardEntityContext> _standardEntityContextMock;
        private Mock<ILogger<PostStandardCommandHandler>> _loggerMock;
        private PostStandardCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _standardEntityContextMock = new Mock<IStandardEntityContext>();
            _loggerMock = new Mock<ILogger<PostStandardCommandHandler>>();

            _sut = new PostStandardCommandHandler(_standardEntityContextMock.Object, _loggerMock.Object);
                
        }

        [Test, AutoMoqData]
        public async Task Handle_ShouldReturnStandardFromDatabase(string title, string reference, int level, string sector)
        {
            // Arrange
            var command = new PostStandardCommand { StandardLevel = level, StandardReference = reference, StandardTitle = title, StandardSector = sector};

            var standard = new Standard { StandardLevel = level, StandardReference = reference, StandardSector = sector, StandardTitle = title };
            _standardEntityContextMock.Setup(x => x.Get(reference)).ReturnsAsync(standard);

            // Act
            var response = await _sut.Handle(command, CancellationToken.None);

            // Assert
            response.Standard.Should().NotBeNull();
            response.Standard.Should().BeEquivalentTo(command);
            _standardEntityContextMock.Verify(x => x.Get(command.StandardReference), Times.Once);
            _standardEntityContextMock.Verify(x => x.Add(It.IsAny<Standard>()), Times.Never);
            _standardEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Test, AutoMoqData]
        public async Task Handle_ShouldCreateStandard(string title, string reference, int level, string sector)
        {
            // Arrange
            var command = new PostStandardCommand { StandardLevel = level, StandardReference = reference, StandardTitle = title, StandardSector = sector };

            // Act
            var response = await _sut.Handle(command, CancellationToken.None);

            // Assert
            response.Standard.Should().NotBeNull();
            response.Standard.Should().BeEquivalentTo(command);
            _standardEntityContextMock.Verify(x => x.Get(command.StandardReference), Times.Once);
            _standardEntityContextMock.Verify(x => x.Add(It.Is<Standard>(s => 
                s.StandardLevel == command.StandardLevel &&
                s.StandardReference == command.StandardReference &&
                s.StandardSector == command.StandardSector &&
                s.StandardTitle == command.StandardTitle
                )), Times.Once);
            _standardEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once); 
        }

        [Test]
        public async Task Handle_ShouldUpdateExistingStandard()
        {
            // Arrange
            var command = new PostStandardCommand { StandardLevel = 1, StandardReference = "ST1024", StandardTitle = "Standard Title", StandardSector = "Standard Sector" };

            var standard = new Standard { StandardLevel = 2, StandardReference = command.StandardReference, StandardSector = "Another sector", StandardTitle = "Another title" };
            _standardEntityContextMock.Setup(x => x.Get(command.StandardReference)).ReturnsAsync(standard);

            // Act
            var response = await _sut.Handle(command, CancellationToken.None);

            // Assert
            response.Standard.Should().NotBeNull();
            response.Standard.Should().BeEquivalentTo(command);
            _standardEntityContextMock.Verify(x => x.Get(command.StandardReference), Times.Once);
            _standardEntityContextMock.Verify(x => x.Add(It.IsAny<Standard>()), Times.Never);
            _standardEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


    }
}
