using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RequestApprenticeTraining.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class EmployerRequestRegionConfiguration : IEntityTypeConfiguration<EmployerRequestRegion>
    {
        public void Configure(EntityTypeBuilder<EmployerRequestRegion> builder)
        {
            builder.ToTable(nameof(EmployerRequestRegion))
                .HasKey(nameof(EmployerRequestRegion.EmployerRequestId), nameof(EmployerRequestRegion.RegionId));
        }
    }
}