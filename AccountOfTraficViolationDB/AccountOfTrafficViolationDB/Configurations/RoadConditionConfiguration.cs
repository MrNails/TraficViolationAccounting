using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTrafficViolationDB.Configurations;

public class RoadConditionConfiguration : IEntityTypeConfiguration<RoadCondition>
{
    public void Configure(EntityTypeBuilder<RoadCondition> builder)
    {
        builder.HasKey(e => e.CaseId)
            .HasName("PK_RoadCondition_CaseId");

        builder.ToTable("RoadCondition");

        builder.Property(e => e.CaseId).ValueGeneratedNever();

        builder.Property(e => e.EngineeringTransportEquipment)
            .IsRequired()
            .HasMaxLength(1);

        builder.Property(e => e.PlaceElement)
            .IsRequired()
            .HasMaxLength(6);

        builder.Property(e => e.RoadDisadvantages)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.SurfaceState)
            .IsRequired()
            .HasMaxLength(2)
            .IsUnicode(false)
            .IsFixedLength();

        builder.Property(e => e.TechnicalTool)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasOne(d => d.Case)
            .WithOne(p => p.RoadCondition)
            .HasForeignKey<RoadCondition>(d => d.CaseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RoadConditionCase_CaseId");
    }
}