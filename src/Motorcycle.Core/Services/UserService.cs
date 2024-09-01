using Motorcycle.Core.Repositories;

namespace Motorcycle.Core.Services;

public interface IUserService
{
    Task<string> GenerateToken(string email, string password, CancellationToken cancellationToken);
}

public class UserService : IUserService
{
    private readonly IJwtBearerService _jwtBearerService;
    private readonly IUserRepository _userRepository;

    public UserService(IJwtBearerService jwtBearerService, IUserRepository userRepositoryepository)
    {
        _jwtBearerService = jwtBearerService;
        _userRepository = userRepositoryepository;
    }

    public async Task<string> GenerateToken(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmail(email, password, cancellationToken);
        var token = _jwtBearerService.GenerateToken(user);
        return await Task.FromResult(token);
    }
}