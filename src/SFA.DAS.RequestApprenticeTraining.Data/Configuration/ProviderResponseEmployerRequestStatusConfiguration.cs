using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RequestApprenticeTraining.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ProviderResponseEmployerRequestStatusConfiguration : IEntityTypeConfiguration<ProviderResponseEmployerRequestStatus>
    {
        public void Configure(EntityTypeBuilder<ProviderResponseEmployerRequestStatus> builder)
        {
            builder.ToTable(nameof(ProviderResponseEmployerRequestStatus))
                .HasKey(nameof(ProviderResponseEmployerRequestStatus.Id));

            builder
                .HasOne(er => er.EmployerRequest)
                .WithMany(er => er.ProviderResponseEmployerRequestsStatus)
                .HasForeignKey(er => er.EmployerRequestId);
        }
    }
}