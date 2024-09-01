using Microsoft.EntityFrameworkCore;
using Motorcycle.Core.Entities;
using Motorcycle.Core.Repositories;
using Motorcycle.Shared.Dtos;
using Motorcycle.Shared.Exceptions;

namespace Motorcycle.Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Guid> Create(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        return user.Id;
    }

    public async Task<User> GetByEmail(string email, string password, CancellationToken cancellationToken)
    {
        return await _context.Users.SingleOrDefaultAsync(e => e.Email == email && e.Password == password,
            cancellationToken) ?? throw new NotFoundException($"User {email} not found");
    }

    public async Task<List<UserDto>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Users
            .Select(e => new UserDto(e.Email, e.Name, e.Role))
            .ToListAsync(cancellationToken);
    }
}