using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTrafficViolationDB.Configurations;

public class GeneralInfoConfiguration : IEntityTypeConfiguration<GeneralInfo>
{
    public void Configure(EntityTypeBuilder<GeneralInfo> builder)
    {
        builder.HasKey(e => e.CaseId)
            .HasName("PK_GeneralInfo_CaseId");

        builder.ToTable("GeneralInfo");

        builder.Property(e => e.CaseId).ValueGeneratedNever();

        builder.Property(e => e.CardNumber)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.FillDate).HasColumnType("date");

        builder.Property(e => e.IncidentDate).HasColumnType("date");

        builder.HasOne(d => d.Case)
            .WithOne(p => p.GeneralInfo)
            .HasForeignKey<GeneralInfo>(d => d.CaseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_GeneralInfoCase_CaseId");
    }
}