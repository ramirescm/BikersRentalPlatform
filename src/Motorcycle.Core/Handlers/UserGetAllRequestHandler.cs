using MediatR;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.UoW;
using Motorcycle.Shared.Requests;
using OperationResult;

namespace Motorcycle.Core.Handlers;

public class UserGetAllRequestHandler : IRequestHandler<UserGetAllRequest, Result<UserGetAllResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepository;

    public UserGetAllRequestHandler(IUnitOfWork uow, IUserRepository userRepository)
    {
        _uow = uow;
        _userRepository = userRepository;
    }

    public async Task<Result<UserGetAllResponse>> Handle(UserGetAllRequest request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAll(cancellationToken);
        return Result.Success(new UserGetAllResponse(users));
    }
}