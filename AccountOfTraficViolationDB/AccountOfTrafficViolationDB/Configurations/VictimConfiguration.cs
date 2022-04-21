using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTrafficViolationDB.Configurations;

public class VictimConfiguration : IEntityTypeConfiguration<Victim>
{
    public void Configure(EntityTypeBuilder<Victim> builder)
    {
        builder.HasKey(e => new { e.CaseId, e.Id })
            .HasName("PK_Victim_VictimCaseId");

        builder.ToTable("Victim");

        builder.Property(e => e.Citizenship)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(e => e.IsDied)
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Patronymic)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Surname)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.TORSerialNumber).HasColumnName("TORSerialNumber");

        builder.HasOne(d => d.Case)
            .WithMany(p => p.Victims)
            .HasForeignKey(d => d.CaseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_VictimCase_CaseId");
    }
}