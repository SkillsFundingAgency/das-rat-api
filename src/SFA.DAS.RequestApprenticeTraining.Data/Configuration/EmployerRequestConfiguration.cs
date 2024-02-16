using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.RequestApprenticeTraining.Domain.Entities;

namespace SFA.DAS.RequestApprenticeTraining.Data.Configuration
{
    public class EmployerRequestConfiguration : IEntityTypeConfiguration<Domain.Entities.EmployerRequest>
    {
        public void Configure(EntityTypeBuilder<EmployerRequest> builder)
        {
            builder.ToTable("EmployerRequest")
                .HasKey(x => x.Id);
        }
    }
}