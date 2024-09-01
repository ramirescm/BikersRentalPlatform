using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Motorcycle.Core.Entities;

namespace Motorcycle.Data;

[ExcludeFromCodeCoverage]
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Deliverer> Deliverers { get; set; }
    public DbSet<MotorcycleRental> MotorcycleRentals { get; set; }
    public DbSet<Core.Entities.Motorcycle> Motorcycles { get; set; }
    public DbSet<RentalPlan?> RentalPlans { get; set; }
    public DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Convert all DateTimeOffset properties to UTC before saving
        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State is EntityState.Added or EntityState.Modified))
        foreach (var property in entry.Properties)
            if (property.Metadata.ClrType == typeof(DateTimeOffset) && property.CurrentValue is DateTimeOffset dto)
                property.CurrentValue = dto.ToUniversalTime();

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        HandleNames(modelBuilder);

        // Set default column type for all string properties to VARCHAR(100)
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        foreach (var property in entityType.GetProperties())
            if (property.ClrType == typeof(string))
                property.SetColumnType("VARCHAR(100)");
    }

    // Set default snake_case
    private void HandleNames(ModelBuilder modelBuilder)
    {
        string ToSnakeCase(string name)
        {
            return Regex
                .Replace(
                    name,
                    @"([a-z0-9])([A-Z])",
                    "$1_$2",
                    RegexOptions.Compiled)
                .ToLower();
        }

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Set table name
            entity.SetTableName(ToSnakeCase(entity.GetTableName()));

            foreach (var property in entity.GetProperties())
                // Set column name
                property.SetColumnName(ToSnakeCase(property.Name));

            foreach (var key in entity.GetKeys())
                // Set key name
                key.SetName(ToSnakeCase(key.GetName()));

            foreach (var key in entity.GetForeignKeys())
                // Set foreign key name
                key.SetConstraintName(ToSnakeCase(key.GetConstraintName()));

            foreach (var index in entity.GetIndexes())
                // Set index name
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
        }
    }
}