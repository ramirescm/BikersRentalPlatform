using Microsoft.EntityFrameworkCore;
using Motorcycle.Core.Repositories;
using Motorcycle.Shared.Exceptions;
using Motorcycle.Shared.Models;

namespace Motorcycle.Data.Repositories;

public class MotorycycleRepository : Repository<Core.Entities.Motorcycle>, IMotorycycleRepository
{
    public MotorycycleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Guid> Add(Core.Entities.Motorcycle motorcycle, CancellationToken cancellationToken)
    {
        await _context.Motorcycles.AddAsync(motorcycle, cancellationToken);
        return motorcycle.Id;
    }

    public async Task<bool> LicensePlateExistsAsync(string licensePlate, CancellationToken cancellationToken)
    {
        return await _context.Motorcycles
            .Where(d => d.LicensePlate == licensePlate)
            .AsNoTracking()
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> UpdateLicensePlate(Guid id, string licensePlate, CancellationToken cancellationToken)
    {
        var motorcycle = await _context.Motorcycles
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (motorcycle is null) throw new NotFoundException("Motorcycle not found.");

        motorcycle.LicensePlate = licensePlate;
        _context.Motorcycles.Update(motorcycle);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var affectedRows = await _context.Database
            .ExecuteSqlRawAsync("DELETE FROM motorcycles WHERE Id = {0}",
                new object[] { id },
                cancellationToken);
        return affectedRows > 0;
    }

    public async Task<List<MotorcycleDto>> GetAll(string? licensePlate, CancellationToken cancellationToken)
    {
        var query = _context.Motorcycles.AsQueryable();
        if (!string.IsNullOrEmpty(licensePlate)) query = query.Where(v => v.LicensePlate == licensePlate);

        return await query
            .Select(e => new MotorcycleDto
            {
                Id = e.Id,
                Year = e.Year,
                Model = e.Model,
                LicensePlate = e.LicensePlate
            })
            .ToListAsync(cancellationToken);
    }
}