﻿using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SFA.DAS.RequestApprenticeTraining.Data;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Application.UnitTests
{
    public static class AutofixtureExtensions
    {
        public static IFixture RequestApprenticeTrainingFixture()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customize(new RequestApprenticeTrainingCustomization());
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            return fixture;
        }
    }

    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public static IDateTimeProvider DateTimeProvider { get; set; }

        public AutoMoqDataAttribute()
            : base(AutofixtureExtensions.RequestApprenticeTrainingFixture)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AutoMoqInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public AutoMoqInlineAutoDataAttribute(params object[] arguments)
            : base(() => AutofixtureExtensions.RequestApprenticeTrainingFixture(), arguments)
        {
        }
    }

    public class RequestApprenticeTrainingCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Customizations.Add(new ApplicationSettingsBuilder());
            fixture.Customizations.Add(new RequestApprenticeTrainingDataContextBuilder());
            fixture.Customizations.Add(new DbContextOptionsBuilder());
        }
    }

    public class RequestApprenticeTrainingDataContextBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(RequestApprenticeTrainingDataContext))
            {
                var applicationSettings = context.Resolve(typeof(IOptions<ApplicationSettings>)) as IOptions<ApplicationSettings>;
                var dbContextOptions = context.Resolve(typeof(DbContextOptions<RequestApprenticeTrainingDataContext>)) as DbContextOptions<RequestApprenticeTrainingDataContext>;

                return GetRequestApprenticeTrainingDataContext(applicationSettings, dbContextOptions);
            }

            return new NoSpecimen();
        }

        public static RequestApprenticeTrainingDataContext GetRequestApprenticeTrainingDataContext(
            IOptions<ApplicationSettings> applicationSettings,
            DbContextOptions<RequestApprenticeTrainingDataContext> dbContextOptions)
        {
            var dbContext = new RequestApprenticeTrainingDataContext(
                applicationSettings,
                dbContextOptions);

            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }

    public class DbContextOptionsBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(DbContextOptions<RequestApprenticeTrainingDataContext>))
            {
                var connection = new SqliteConnection("Filename=:memory:");
                connection.Open();

                // These options will be used by the context instances in this test suite, including the connection opened above.
                var contextOptions = new DbContextOptionsBuilder<RequestApprenticeTrainingDataContext>()
                    .UseSqlite(connection)
                    .Options;
                
                return contextOptions;
            }

            return new NoSpecimen();
        }
    }

    public class ApplicationSettingsBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(IOptions<ApplicationSettings>))
            {
                var applicationSettings = new ApplicationSettings
                {
                    NotificationTemplates = new System.Collections.Generic.List<NotificationTemplate>
                    {
                        new NotificationTemplate { TemplateName = "SomeTemplate", TemplateId = Guid.NewGuid() }
                    }
                };

                return Options.Create(applicationSettings);
            }

            return new NoSpecimen();
        }
    }
}
