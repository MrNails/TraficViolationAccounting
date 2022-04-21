using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTrafficViolationDB.Configurations;

public class CaseVehicleConfiguration : IEntityTypeConfiguration<CaseVehicle>
{
    public void Configure(EntityTypeBuilder<CaseVehicle> builder)
    {
        builder.ToTable("CaseVehicle");
        
        builder.HasKey(cv => new { cv.CaseId, cv.VehicleId })
            .HasName("PK_CaseVehicle_CaseVehicleId");

        builder.Property(e => e.ActivityLicensingInfo).HasMaxLength(2);

        builder.Property(e => e.ChasisNumber)
            .IsRequired()
            .HasMaxLength(9);

        builder.Property(e => e.CorruptionCode).HasMaxLength(8);

        builder.Property(e => e.DriverSurname)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.FrameNumber)
            .IsRequired()
            .HasMaxLength(8);

        builder.Property(e => e.LicenceNumber)
            .IsRequired()
            .HasMaxLength(6);

        builder.Property(e => e.LicenceSeries)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(e => e.PlateNumber)
            .IsRequired()
            .HasMaxLength(9);

        builder.Property(e => e.TechnicalFaults)
            .IsRequired()
            .HasMaxLength(2);

        builder.HasOne(d => d.Case)
            .WithMany(p => p.CaseVehicles)
            .HasForeignKey(d => d.CaseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_CaseVehicle_CaseId");

        builder.HasOne(d => d.Vehicle)
            .WithMany(p => p.CaseVehicles)
            .HasForeignKey(d => d.VehicleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_CaseVehicle_VehicleId");
    }
}