using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTrafficViolationDB.Configurations;

public class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Created).HasDefaultValueSql("(getdate())");

        builder.Property(e => e.Modified).HasDefaultValueSql("(getdate())");

        builder.HasOne(d => d.Officer)
            .WithMany(p => p.Cases)
            .HasForeignKey(d => d.OfficerId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_CaseOfficer_OfficerId");
    }
}