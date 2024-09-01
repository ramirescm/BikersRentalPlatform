using Motorcycle.Core.Entities;
using Motorcycle.Shared.Dtos;

namespace Motorcycle.Core.Repositories;

public interface IUserRepository
{
    Task<Guid> Create(User user, CancellationToken cancellationToken);
    Task<User> GetByEmail(string email, string password, CancellationToken cancellationToken);
    Task<List<UserDto>> GetAll(CancellationToken cancellationToken);
}