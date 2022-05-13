using AccountOfTraficViolationDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountOfTraficViolationDB.Configurations;

public class CodeBindingConfiguration : IEntityTypeConfiguration<CodeBinding>
{
    public void Configure(EntityTypeBuilder<CodeBinding> builder)
    {
        builder.HasKey(cb => cb.Id)
            .HasName("PK_CodeBindings_Id");
        
        builder.Property(cb => cb.Id).ValueGeneratedNever();

        builder.Property(cb => cb.Name)
            .HasMaxLength(50);

        builder.Property(cb => cb.Description)
            .HasMaxLength(500);
    }
}