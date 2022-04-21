using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTrafficViolationDB.Configurations;

public class AccidentOnHighwayConfiguration: IEntityTypeConfiguration<AccidentOnHighway>
{
    public void Configure(EntityTypeBuilder<AccidentOnHighway> builder)
    {
        builder.ToTable("AccidentOnHighway");

        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.AdditionalInfo)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.HighwayBinding)
            .IsRequired()
            .HasMaxLength(47);

        builder.Property(e => e.HighwayIndexAndNumber)
            .IsRequired()
            .HasMaxLength(6);

        builder.Property(e => e.Kilometer)
            .IsRequired()
            .HasMaxLength(4);

        builder.Property(e => e.Meter)
            .IsRequired()
            .HasMaxLength(3);
    }
}