using MediatR;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Shared.Requests;

public record MotorcycleRentChangeEndDateRequest(Guid Id, DateTimeOffset ActualReturnDate)
    : IRequest<Result<MotorcycleRentChangeEndDateResponse>>;