using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.UnitTests;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.Commands.RefreshStandards.UnitTests
{
    [TestFixture]
    public class RefreshStandardsCommandHandlerTests
    {
        private Mock<IStandardEntityContext> _standardEntityContextMock;
        private Mock<ILogger<RefreshStandardsCommandHandler>> _loggerMock;
        private RefreshStandardsCommandHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _standardEntityContextMock = new Mock<IStandardEntityContext>();
            _loggerMock = new Mock<ILogger<RefreshStandardsCommandHandler>>();

            _sut = new RefreshStandardsCommandHandler(_standardEntityContextMock.Object);
        }

        [Test, AutoMoqData]
        public async Task Handle_ShouldInsertNewStandard()
        {
            //Arrange
            var newStandardReference = "ST0059";
            var newLevel = 5;
            var newTitle = "Boatbuilder";
            var newSector = "Engineering and manufacturing";

            
            var command = new RefreshStandardsCommand
            {
                Standards = new List<Domain.Models.StandardsParameter> 
                {
                    new Domain.Models.StandardsParameter
                    {
                        StandardReference = "ST0004",
                        StandardLevel = 3,
                        StandardTitle = "Actuarial technician",
                        StandardSector= "Legal, finance and accounting"
                    },
                    new Domain.Models.StandardsParameter
                    {
                        StandardReference = newStandardReference,
                        StandardLevel = 5,
                        StandardTitle = "Boatbuilder",
                        StandardSector = "Engineering and manufacturing"
                    }
                }
            };
            var entitiesInDb = new List<Standard> 
            {
                new Standard
                {
                    StandardReference = "ST0004",
                    StandardLevel = 3,
                    StandardTitle = "Actuarial technician",
                    StandardSector= "Legal, finance and accounting"
                }
            };
            _standardEntityContextMock.Setup(c => c.GetAll()).ReturnsAsync(entitiesInDb);

            // Act
            var response = await _sut.Handle(command, CancellationToken.None);

            // Assert
            _standardEntityContextMock.Verify(x => x.GetAll(), Times.Once);
            _standardEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _standardEntityContextMock.Verify(x =>
                 x.AddRange(It.Is<List<Standard>>(list =>
                     list.Count == 1 &&
                     list[0].StandardReference == newStandardReference &&  
                     list[0].StandardTitle == newTitle &&  
                     list[0].StandardLevel == newLevel &&             
                     list[0].StandardSector == newSector  
                 )), Times.Once);

        }


        [Test, AutoMoqData]
        public async Task Handle_ShouldUpdateExistingStandard()
        {
            //Arrange
            var referenceToUpdate = "ST0001";
            var updatedTitle = "The new title";
            var updatedLevel = 8;
            var updatedSector = "The new sector";

            var command = new RefreshStandardsCommand
            {
                Standards = new List<Domain.Models.StandardsParameter>
                {
                    new Domain.Models.StandardsParameter
                    {
                        StandardReference = referenceToUpdate,
                        StandardLevel = updatedLevel,
                        StandardTitle = updatedTitle,
                        StandardSector= updatedSector,
                    },
                    new Domain.Models.StandardsParameter
                    {
                        StandardReference = "ST0004",
                        StandardLevel = 3,
                        StandardTitle = "Actuarial technician",
                        StandardSector = "Legal, finance and accounting"
                    }
                }
            };
            var entitiesInDb = new List<Standard>
            {
                new Standard
                {
                    StandardReference = referenceToUpdate,
                    StandardLevel = 7,
                    StandardTitle = "Accountancy or taxation professional",
                    StandardSector= "Legal, finance and accounting"
                },new Standard
                {
                    StandardReference = "ST0004",
                    StandardLevel = 3,
                    StandardTitle = "Actuarial technician",
                    StandardSector= "Legal, finance and accounting"
                }
            };
            _standardEntityContextMock.Setup(c => c.GetAll()).ReturnsAsync(entitiesInDb);

            // Act
            var response = await _sut.Handle(command, CancellationToken.None);

            // Assert
            _standardEntityContextMock.Verify(x => x.GetAll(), Times.Once);
            _standardEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _standardEntityContextMock.Verify(x => x.AddRange(It.Is<List<Standard>>(list => list.Count == 0)), Times.Once);

            var updatedStandard = entitiesInDb.FirstOrDefault(s => s.StandardReference == referenceToUpdate);
            updatedStandard.StandardTitle.Should().Be(updatedTitle);
            updatedStandard.StandardLevel.Should().Be(updatedLevel);
            updatedStandard.StandardSector.Should().Be(updatedSector);  
        }

        [Test, AutoMoqData]
        public async Task Handle_ShouldHandleEmptyData()
        {
            //Arrange
            var command = new RefreshStandardsCommand
            {
                Standards = new List<Domain.Models.StandardsParameter>()
            };

            var entitiesInDb = new List<Standard>();

            _standardEntityContextMock.Setup(c => c.GetAll()).ReturnsAsync(entitiesInDb);

            // Act
            var response = await _sut.Handle(command, CancellationToken.None);

            // Assert
            _standardEntityContextMock.Verify(x => x.GetAll(), Times.Once);
            _standardEntityContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _standardEntityContextMock.Verify(x => x.AddRange(It.Is<List<Standard>>(list => list.Count == 0)), Times.Once);
        }
    }
}
