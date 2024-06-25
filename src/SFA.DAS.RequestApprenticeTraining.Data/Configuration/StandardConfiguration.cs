using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RequestApprenticeTraining.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class StandardConfiguration : IEntityTypeConfiguration<Standard>
    {
        public void Configure(EntityTypeBuilder<Standard> builder)
        {
            builder.ToTable(nameof(Standard))
                .HasKey(nameof(Standard.StandardReference));
        }
    }
}