using Motorcycle.Core.Entities;
using Motorcycle.Shared.Dtos;

namespace Motorcycle.Core.Repositories;

public interface IDelivererRepository
{
    Task<Guid> Add(Deliverer deliverer, CancellationToken cancellationToken);
    Task<bool> CnhExistsAsync(string cnh, CancellationToken cancellationToken);
    Task<bool> CnpjExistsAsync(string cnpj, CancellationToken cancellationToken);

    Task<bool> CnhAllowedExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<Deliverer> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> UpdateImageCnhAsync(Guid id, string newImagePath);

    Task<List<DelivererDto>> GetAll(string? cnh, CancellationToken cancellationToken);
}