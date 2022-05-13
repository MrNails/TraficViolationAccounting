using AccountingOfTrafficViolation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTraficViolationDB.Configurations;

public class CodeConfiguration : IEntityTypeConfiguration<Code>
{
    public void Configure(EntityTypeBuilder<Code> builder)
    {
        builder.HasKey(cb => cb.Id)
            .HasName("PK_Codes_Id");
        
        builder.Property(cb => cb.Id).ValueGeneratedNever();

        builder.Property(cb => cb.Name)
            .HasMaxLength(50);

        builder.Property(cb => cb.Description)
            .HasMaxLength(500);

        builder.HasOne(c => c.CodeBinding)
            .WithOne(c => c.Code)
            .HasForeignKey<Code>(d => d.CodeBindingId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Codes_CodeBindingId");
    }
}