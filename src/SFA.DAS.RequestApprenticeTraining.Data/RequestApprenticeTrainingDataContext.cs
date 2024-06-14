using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SFA.DAS.RequestApprenticeTraining.Data.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Configuration;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using SFA.DAS.RequestApprenticeTraining.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.RequestApprenticeTraining.Data
{
    [ExcludeFromCodeCoverage]
    public class RequestApprenticeTrainingDataContext : DbContext,
        IEmployerRequestEntityContext,
        IEmployerRequestRegionEntityContext,
        IRegionEntityContext,
        IRequestTypeEntityContext
    {
        private const string AzureResource = "https://database.windows.net/";
        private readonly ApplicationSettings _configuration;
        private readonly ChainedTokenCredential _chainedTokenCredentialProvider;

        public virtual DbSet<EmployerRequest> EmployerRequests { get; set; }
        public virtual DbSet<EmployerRequestRegion> EmployerRequestRegions { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<RequestType> RequestTypes { get; set; }

        DbSet<EmployerRequest> IEntityContext<EmployerRequest>.Entities => EmployerRequests;
        DbSet<EmployerRequestRegion> IEntityContext<EmployerRequestRegion>.Entities => EmployerRequestRegions;
        DbSet<RequestType> IEntityContext<RequestType>.Entities => RequestTypes;
        DbSet<Region> IEntityContext<Region>.Entities => Regions;

        public RequestApprenticeTrainingDataContext(IOptions<ApplicationSettings> config,
            DbContextOptions<RequestApprenticeTrainingDataContext> options)
            : base(options)
        {
            _configuration = config.Value;
        }

        public RequestApprenticeTrainingDataContext(IOptions<ApplicationSettings> config,
            DbContextOptions<RequestApprenticeTrainingDataContext> options,
            ChainedTokenCredential chainedTokenCredentialProvider) 
            : base(options)
        {
            _configuration = config.Value;
            _chainedTokenCredentialProvider = chainedTokenCredentialProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_chainedTokenCredentialProvider != null)
            {
                var connection = new SqlConnection
                {
                    ConnectionString = _configuration.DbConnectionString,
                    AccessToken = _chainedTokenCredentialProvider
                        .GetTokenAsync(new TokenRequestContext(scopes: [AzureResource]))
                        .GetAwaiter().GetResult().Token
                };

                optionsBuilder.UseSqlServer(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployerRequestConfiguration());
            modelBuilder.ApplyConfiguration(new EmployerRequestRegionConfiguration());
            modelBuilder.ApplyConfiguration(new RegionConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public async Task<Region> FindClosestRegion(double latitude, double longitude)
        {
            var results = await Set<Region>()
                .FromSqlRaw("EXEC FindClosestRegion @latitude = {0}, @longitude = {1}", latitude, longitude)
                .AsNoTracking()
                .ToListAsync();

            return results.FirstOrDefault();
        }
    }
}
