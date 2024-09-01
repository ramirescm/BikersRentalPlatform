using Microsoft.EntityFrameworkCore;
using Motorcycle.Core.Entities;
using Motorcycle.Core.Repositories;
using Motorcycle.Shared.Dtos;
using Motorcycle.Shared.Exceptions;

namespace Motorcycle.Data.Repositories;

public class MotorcycleRentalPlanRepository : Repository<RentalPlan>, IMotorcycleRentalPlanRepository
{
    public MotorcycleRentalPlanRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Guid> CreateRental(MotorcycleRental motorcycleRental, CancellationToken cancellationToken)
    {
        await _context.MotorcycleRentals.AddAsync(motorcycleRental, cancellationToken);
        return motorcycleRental.Id;
    }

    public async Task<MotorcycleRental> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _context.MotorcycleRentals.AsNoTracking()
                   .SingleOrDefaultAsync(e => e.Id == id, cancellationToken) ??
               throw new NotFoundException("Rental not exists");
    }

    public async Task<List<MotorcycleRentalDto>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.MotorcycleRentals
            .Select(e => new MotorcycleRentalDto
            {
                Id = e.Id,
                MotorcycleId = e.MotorcycleId,
                DelivererId = e.DelivererId,
                RentalPlanId = e.RentalPlanId,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                ExpectedFinishDate = e.ExpectedFinishDate,
                CreatedAt = e.CreatedAt,
                AmountPredicted = e.AmountPredicted,
                AmountPaid = e.AmountPaid
            })
            .ToListAsync(cancellationToken);
    }
}