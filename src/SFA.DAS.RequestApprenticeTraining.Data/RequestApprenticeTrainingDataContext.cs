using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using SFA.DAS.RequestApprenticeTraining.Data.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Data
{
    public class RequestApprenticeTrainingDataContext : DbContext,
        IEmployerRequestEntityContext,
        IRequestTypeEntityContext
    {
        private const string AzureResource = "https://database.windows.net/";
        private readonly ApplicationSettings _configuration;
        private readonly ChainedTokenCredential _chainedTokenCredentialProvider;
        private readonly IDateTimeProvider _dateTimeHelper;

        public virtual DbSet<EmployerRequest> EmployerRequests { get; set; }
        public virtual DbSet<RequestType> RequestTypes { get; set; }

        DbSet<EmployerRequest> IEntityContext<EmployerRequest>.Entities => EmployerRequests;
        DbSet<RequestType> IEntityContext<RequestType>.Entities => RequestTypes;

        public RequestApprenticeTrainingDataContext(IOptions<ApplicationSettings> config,
            DbContextOptions<RequestApprenticeTrainingDataContext> options,
            IDateTimeProvider dateTimeHelper)
            : base(options)
        {
            _configuration = config.Value;
            _dateTimeHelper = dateTimeHelper;
        }

        public RequestApprenticeTrainingDataContext(IOptions<ApplicationSettings> config,
            DbContextOptions<RequestApprenticeTrainingDataContext> options,
            ChainedTokenCredential chainedTokenCredentialProvider,
            IDateTimeProvider dateTimeHelper) 
            : base(options)
        {
            _configuration = config.Value;
            _chainedTokenCredentialProvider = chainedTokenCredentialProvider;
            _dateTimeHelper = dateTimeHelper;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_chainedTokenCredentialProvider != null)
            {
                var connection = new SqlConnection
                {
                    ConnectionString = _configuration.DbConnectionString,
                    AccessToken = _chainedTokenCredentialProvider.GetTokenAsync(new TokenRequestContext(scopes: new string[] { AzureResource })).Result.Token
                };

                optionsBuilder.UseSqlServer(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployerRequestConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(
           bool acceptAllChangesOnSuccess,
           CancellationToken cancellationToken = default
        )
        {
            OnBeforeSaving();
            return (await base.SaveChangesAsync(acceptAllChangesOnSuccess,
                          cancellationToken));
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = _dateTimeHelper.Now;

            foreach (var entry in entries)
            {
                // for entities that inherit from BaseEntity,
                // set UpdatedOn / CreatedOn appropriately
                if (entry.Entity is EntityBase trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            // set the updated date to "now"
                            trackable.UpdatedOn = utcNow;

                            // mark property as "don't touch"
                            // we don't want to update on a Modify operation
                            entry.Property("CreatedOn").IsModified = false;
                            break;

                        case EntityState.Added:
                            // set both updated and created date to "now"
                            trackable.CreatedOn = utcNow;
                            trackable.UpdatedOn = utcNow;
                            break;
                    }

                }
            }
        }
    }
}
