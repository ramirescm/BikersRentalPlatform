using Microsoft.EntityFrameworkCore;
using Motorcycle.Core.Entities;
using Motorcycle.Core.Repositories;
using Motorcycle.Shared.Exceptions;
using Motorcycle.Shared.Models;

namespace Motorcycle.Data.Repositories;

public class RentalPlanRepository : Repository<RentalPlan>, IRentalPlanRepository
{
    public RentalPlanRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> RentalPlanMotorcycleExistsAsync(Guid motorcycleId, CancellationToken cancellationToken)
    {
        return await _context.MotorcycleRentals.AsNoTracking()
            .AnyAsync(e => e.MotorcycleId == motorcycleId, cancellationToken);
    }

    public async Task<RentalPlan> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _context.RentalPlans.AsNoTracking()
                   .SingleOrDefaultAsync(e => e.Id == id, cancellationToken) ??
               throw new NotFoundException("Rental Plan not exists");
    }

    public async Task<List<RentalPlanDto>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.RentalPlans
            .Select(e => new RentalPlanDto
            {
                Id = e.Id,
                Amount = e.Amount,
                Fee = e.Fee,
                Days = e.Days
            })
            .ToListAsync(cancellationToken);
    }
}