using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTrafficViolationDB.Configurations;

public class AccidentOnVillageConfiguration : IEntityTypeConfiguration<AccidentOnVillage>
{
    public void Configure(EntityTypeBuilder<AccidentOnVillage> builder)
    {
        builder.ToTable("AccidentOnVillage");

        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.District)
            .IsRequired()
            .HasMaxLength(22);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(22);

        builder.Property(e => e.Street)
            .IsRequired()
            .HasMaxLength(22);

        builder.Property(e => e.VillageBinding)
            .IsRequired()
            .HasMaxLength(47);
    }
}