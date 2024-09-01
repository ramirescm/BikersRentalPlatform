using MediatR;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Shared.Requests;

public record MotorcycleCreateRequest(int Year, string Model, string LicensePlate)
    : IRequest<Result<MotorcycleCreateResponse>>;