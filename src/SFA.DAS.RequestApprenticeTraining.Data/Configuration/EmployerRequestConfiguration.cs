using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.RequestApprenticeTraining.Domain.Models;
using System;

namespace SFA.DAS.RequestApprenticeTraining.Data.Configuration
{
    public class EmployerRequestConfiguration : IEntityTypeConfiguration<Domain.Entities.EmployerRequest>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.EmployerRequest> builder)
        {
            builder.ToTable("EmployerRequest")
            .HasKey(x => x.Id);

            builder.Property(e => e.RequestType)
               .HasConversion<int>()
               .HasColumnName("RequestTypeId");
        }
    }
}