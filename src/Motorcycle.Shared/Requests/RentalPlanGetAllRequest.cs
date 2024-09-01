using MediatR;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Shared.Requests;

public record RentalPlanGetAllRequest : IRequest<Result<RentalPlanGetAllResponse>>;