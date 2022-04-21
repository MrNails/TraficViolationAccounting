using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTrafficViolationDB.Configurations;

public class CaseAccidentPlaceConfiguration : IEntityTypeConfiguration<CaseAccidentPlace>
{
    public void Configure(EntityTypeBuilder<CaseAccidentPlace> builder)
    {
        builder.HasNoKey();

        builder.ToTable("CaseAccidentPlace");

        builder.HasIndex(e => new { e.CaseId, e.AccidentOnHighwayId, e.AccidentOnVillageId }, "IX_CaseAccidentPlace_AllId")
            .IsClustered();

        builder.HasOne(d => d.AccidentOnHighway)
            .WithMany()
            .HasForeignKey(d => d.AccidentOnHighwayId)
            .HasConstraintName("FK_CaseAccidentPlace_AccidentOnHighwayId");

        builder.HasOne(d => d.AccidentOnVillage)
            .WithMany()
            .HasForeignKey(d => d.AccidentOnVillageId)
            .HasConstraintName("FK_CaseAccidentPlace_AccidentOnVillageId");

        builder.HasOne(d => d.Case)
            .WithMany()
            .HasForeignKey(d => d.CaseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_CaseAccidentPlace_CaseId");
    }
}