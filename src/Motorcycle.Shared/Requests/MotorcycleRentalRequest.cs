using MediatR;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Shared.Requests;

public record MotorcycleRentalRequest(
    Guid MotorcycleId,
    Guid DelivererId,
    Guid RentalPlanId) : IRequest<Result<MotorcycleRentalResponse>>;