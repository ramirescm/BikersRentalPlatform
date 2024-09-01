using MediatR;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Shared.Requests;

public record MotorcycleGetAllRequest(string? LicensePlate) : IRequest<Result<MotorcycleGetAllResponse>>;