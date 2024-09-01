using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Motorcycle.Core.Entities;

namespace Motorcycle.Data.Configurations;

public class RentalPlanConfiguration : IEntityTypeConfiguration<RentalPlan>
{
    public void Configure(EntityTypeBuilder<RentalPlan> builder)
    {
        builder.ToTable("rental_plans");
    }
}