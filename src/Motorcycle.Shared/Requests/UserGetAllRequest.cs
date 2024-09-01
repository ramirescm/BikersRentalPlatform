using MediatR;
using OperationResult;

namespace Motorcycle.Shared.Requests;

public record UserGetAllRequest : IRequest<Result<UserGetAllResponse>>;