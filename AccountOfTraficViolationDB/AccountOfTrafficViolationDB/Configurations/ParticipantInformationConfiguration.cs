using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTrafficViolationDB.Configurations;

public class ParticipantInformationConfiguration : IEntityTypeConfiguration<ParticipantsInformation>
{
    public void Configure(EntityTypeBuilder<ParticipantsInformation> builder)
    {
        builder.HasKey(e => new { e.CaseId, e.Id })
            .HasName("PK_ParticipantInformation_ParticipCaseId");

        builder.ToTable("ParticipantInformation");

        builder.Property(e => e.Address)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Citizenship)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Patronymic)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.PDDViolation)
            .IsRequired()
            .HasMaxLength(4)
            .HasColumnName("PDDViolation");

        builder.Property(e => e.Surname)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(d => d.Case)
            .WithMany(p => p.ParticipantsInformations)
            .HasForeignKey(d => d.CaseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ParticipantInformationCase_CaseId");
    }
}