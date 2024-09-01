using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motorcycle.Core.Entities;

namespace Motorcycle.Data.Configurations;

public class DelivererConfiguration : IEntityTypeConfiguration<Deliverer>
{
    public void Configure(EntityTypeBuilder<Deliverer> builder)
    {
        builder.ToTable("deliverers");

        builder.HasKey(d => d.Id);

        // builder.Property(d => d.Id)
        //     .IsRequired();
        //
        // builder.Property(d => d.Name)
        //     .IsRequired()
        //     .HasMaxLength(100);
        //
        // builder.Property(d => d.Cnpj)
        //     .IsRequired()
        //     .HasMaxLength(14); // Standard CNPJ length
        //
        // builder.HasIndex(d => d.Cnpj)
        //     .IsUnique(); // Ensure CNPJ is unique
        //
        // builder.Property(d => d.BirthDate)
        //     .IsRequired();
        //
        // builder.Property(d => d.CnhNumber)
        //     .IsRequired()
        //     .HasMaxLength(11);
        //
        // builder.HasIndex(d => d.CnhNumber)
        //     .IsUnique(); // Ensure CNH Number is unique
        //
        // builder.Property(d => d.CnhType)
        //     .IsRequired()
        //     .HasMaxLength(3) // Allow "A", "B", "A+B"
        //     .HasConversion(
        //         v => v.ToUpper(), // Ensure values are stored in uppercase
        //         v => v); 
        //
        // builder.Property(d => d.CnhPathImage)
        //     .HasMaxLength(200); 
        //
        // // Adding a check constraint for CNH Type to ensure it is A, B, or A+B
        // builder.HasCheckConstraint("CK_Deliverer_CnhType", "CnhType IN ('A', 'B', 'A+B')");
    }
}