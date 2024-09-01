using Motorcycle.Core.Entities;
using Motorcycle.Shared.Models;

namespace Motorcycle.Core.Repositories;

public interface IRentalPlanRepository
{
    Task<bool> RentalPlanMotorcycleExistsAsync(Guid motorcycleId, CancellationToken cancellationToken);
    Task<RentalPlan> GetById(Guid id, CancellationToken cancellationToken);

    Task<List<RentalPlanDto>> GetAll(CancellationToken cancellationToken);
}