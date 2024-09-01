using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motorcycle.Core.Entities;

namespace Motorcycle.Data.Configurations;

public class MotorcycleRentalConfiguration : IEntityTypeConfiguration<MotorcycleRental>
{
    public void Configure(EntityTypeBuilder<MotorcycleRental> builder)
    {
        builder.ToTable("motorcycle_rentals");
    }
}