﻿using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.RequestApprenticeTraining.Application.Queries.GetSettings;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests.Queries.GetSettings
{
    public class WhenHandlingGetSettingsQuery
    {
        private GetSettingsQueryHandler _handler;
        private Mock<IOptions<ApplicationSettings>> _mockOptions;

        [SetUp]
        public void SetUp()
        {
            _mockOptions = new Mock<IOptions<ApplicationSettings>>();
            _mockOptions
                .SetupGet(s => s.Value)
                .Returns(new ApplicationSettings 
                { 
                    ExpiryAfterMonths = 3, 
                    EmployerRemovedAfterExpiryNoResponsesMonths = 3, 
                    EmployerRemovedAfterExpiryResponsesMonths = 3,
                    ProviderRemovedAfterExpiryNotRespondedMonths = 3,
                    ProviderRemovedAfterExpiryRespondedMonths = 9,
                    
                });

            _handler = new GetSettingsQueryHandler(_mockOptions.Object);
        }

        [Test]
        public async Task Then_Returns_Settings()
        {
            // Arrange
            var query = new GetSettingsQuery();
            
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ExpiryAfterMonths.Should().Be(3);
            result.EmployerRemovedAfterExpiryNoResponsesMonths.Should().Be(3);
            result.EmployerRemovedAfterExpiryResponsesMonths.Should().Be(3);
            result.ProviderRemovedAfterExpiryNotRespondedMonths.Should().Be(3);
            result.ProviderRemovedAfterExpiryRespondedMonths.Should().Be(9);
        }
    }
}
