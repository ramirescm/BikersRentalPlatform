using MediatR;
using OperationResult;

namespace Motorcycle.Shared.Requests;

public record UserCreateRequest(string Email, string Name, string Password, string Role)
    : IRequest<Result<UserCreateResponse>>;

public record UserCreateResponse;