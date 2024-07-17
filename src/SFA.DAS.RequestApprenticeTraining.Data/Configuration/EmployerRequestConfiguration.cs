using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RequestApprenticeTraining.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class EmployerRequestConfiguration : IEntityTypeConfiguration<EmployerRequest>
    {
        public void Configure(EntityTypeBuilder<EmployerRequest> builder)
        {
            builder.ToTable(nameof(EmployerRequest))
                .HasKey(x => x.Id);

            builder.Property(e => e.RequestType)
               .HasConversion<int>()
               .HasColumnName("RequestTypeId");

            builder.Property(e => e.RequestStatus)
               .HasConversion<int>()
               .HasColumnName("RequestStatusId");

            builder.HasOne(e => e.Standard)
                .WithMany()
                .HasForeignKey(e => e.StandardReference);

        }
    }
}