using MediatR;
using OperationResult;

namespace Motorcycle.Shared.Requests;

public record MotorcycleDeleteRequest(Guid Id) : IRequest<Result>;