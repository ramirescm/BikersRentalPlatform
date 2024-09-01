using Motorcycle.Core.Entities;
using Motorcycle.Shared.Dtos;

namespace Motorcycle.Core.Repositories;

public interface IMotorcycleRentalPlanRepository
{
    Task<Guid> CreateRental(MotorcycleRental motorcycleRental, CancellationToken cancellationToken);
    Task<MotorcycleRental> GetById(Guid id, CancellationToken cancellationToken);

    Task<List<MotorcycleRentalDto>> GetAll(CancellationToken cancellationToken);
}