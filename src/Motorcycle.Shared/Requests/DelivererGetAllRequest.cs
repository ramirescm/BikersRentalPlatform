using MediatR;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Shared.Requests;

public record DelivererGetAllRequest(string? Cnh) : IRequest<Result<DelivererGetAllResponse>>;