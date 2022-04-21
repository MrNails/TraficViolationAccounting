using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTrafficViolationDB.Configurations;

public class OfficerConfiguration : IEntityTypeConfiguration<Officer>
{
    public void Configure(EntityTypeBuilder<Officer> builder)
    {
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Phone)
            .IsRequired()
            .HasMaxLength(13)
            .IsUnicode(false);

        builder.Property(e => e.Surname)
            .IsRequired()
            .HasMaxLength(100);
    }
}