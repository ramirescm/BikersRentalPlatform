using Motorcycle.Shared.Models;

namespace Motorcycle.Core.Repositories;

public interface IMotorycycleRepository
{
    Task<Guid> Add(Entities.Motorcycle motorcycle, CancellationToken cancellationToken);
    Task<bool> LicensePlateExistsAsync(string licensePlate, CancellationToken cancellationToken);
    Task<bool> UpdateLicensePlate(Guid id, string licensePlate, CancellationToken cancellationToken);
    Task<bool> DeleteById(Guid id, CancellationToken cancellationToken);
    Task<List<MotorcycleDto>> GetAll(string? licensePlate, CancellationToken cancellationToken);
}