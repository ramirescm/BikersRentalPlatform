using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Motorcycle.Data.Configurations;

public class MotorcycleConfiguration : IEntityTypeConfiguration<Core.Entities.Motorcycle>
{
    public void Configure(EntityTypeBuilder<Core.Entities.Motorcycle> builder)
    {
        builder.ToTable("motorcycles");
    }
}