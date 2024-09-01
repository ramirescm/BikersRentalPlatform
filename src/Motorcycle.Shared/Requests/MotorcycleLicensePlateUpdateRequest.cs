using MediatR;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Shared.Requests;

public record MotorcycleLicensePlateUpdateRequest(Guid Id, string LicensePlate)
    : IRequest<Result<MotorcycleLicensePlateUpdateResponse>>;