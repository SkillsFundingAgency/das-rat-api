using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RequestApprenticeTraining.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ProviderResponseEmployerRequestConfiguration : IEntityTypeConfiguration<ProviderResponseEmployerRequest>
    {
        public void Configure(EntityTypeBuilder<ProviderResponseEmployerRequest> builder)
        {
            builder.ToTable(nameof(ProviderResponseEmployerRequest))
                .HasKey(nameof(ProviderResponseEmployerRequest.EmployerRequestId), nameof(ProviderResponseEmployerRequest.Ukprn));

            builder
                .HasOne(er => er.EmployerRequest)
                .WithMany(er => er.ProviderResponseEmployerRequests)
                .HasForeignKey(er => er.EmployerRequestId);

            builder
                .HasOne(er => er.ProviderResponse)
                .WithMany(er => er.ProviderResponseEmployerRequests)
                .HasForeignKey(er => er.ProviderResponseId)
                .IsRequired(false);
        }
    }
}