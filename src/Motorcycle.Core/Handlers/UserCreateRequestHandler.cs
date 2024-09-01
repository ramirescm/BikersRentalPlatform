using MediatR;
using Motorcycle.Core.Entities;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.UoW;
using Motorcycle.Shared.Requests;
using OperationResult;

namespace Motorcycle.Core.Handlers;

public class UserCreateRequestHandler : IRequestHandler<UserCreateRequest, Result<UserCreateResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepository;

    public UserCreateRequestHandler(IUnitOfWork uow, IUserRepository userRepository)
    {
        _uow = uow;
        _userRepository = userRepository;
    }

    public async Task<Result<UserCreateResponse>> Handle(UserCreateRequest request, CancellationToken cancellationToken)
    {
        var id = await _userRepository.Create(
            new User { Email = request.Email, Name = request.Name, Password = request.Password, Role = request.Role },
            cancellationToken);
        await _uow.CommitAsync(cancellationToken);
        return Result.Success(new UserCreateResponse());
    }
}