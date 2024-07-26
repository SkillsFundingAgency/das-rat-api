using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RequestApprenticeTraining.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ProviderResponseConfiguration : IEntityTypeConfiguration<ProviderResponse>
    {
        public void Configure(EntityTypeBuilder<ProviderResponse> builder)
        {
            builder.ToTable(nameof(ProviderResponse))
                .HasKey(nameof(ProviderResponse.Id));
        }
    }
}