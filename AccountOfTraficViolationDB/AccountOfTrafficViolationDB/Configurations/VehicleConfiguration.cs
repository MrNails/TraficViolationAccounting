using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTrafficViolationDB.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicle");

        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.EDRPOU_Code)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("EDRPOU_Code")
            .IsFixedLength();

        builder.Property(e => e.Make)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.Model)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.Owner)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.PolicyEndDate).HasColumnType("date");

        builder.Property(e => e.PolicyNumber)
            .IsRequired()
            .HasMaxLength(6);

        builder.Property(e => e.PolicySeries)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(e => e.RegistrationSertificate)
            .IsRequired()
            .HasMaxLength(6);

        builder.Property(e => e.SeriesOfRegistrationSertificate)
            .IsRequired()
            .HasMaxLength(3);
    }
}