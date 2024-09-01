using MediatR;
using Motorcycle.Core.Repositories;
using Motorcycle.Shared.Requests;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Core.Handlers;

public class MotorcycleRentChangeEndDateRequestHandler : IRequestHandler<MotorcycleRentChangeEndDateRequest,
    Result<MotorcycleRentChangeEndDateResponse>>
{
    private readonly IMotorcycleRentalPlanRepository _motorcycleRentalPlanRepository;
    private readonly IRentalPlanRepository _rentalPlanRepository;

    public MotorcycleRentChangeEndDateRequestHandler(IMotorcycleRentalPlanRepository motorcycleRentalPlanRepository,
        IRentalPlanRepository rentalPlanRepository)
    {
        _motorcycleRentalPlanRepository = motorcycleRentalPlanRepository;
        _rentalPlanRepository = rentalPlanRepository;
    }

    public async Task<Result<MotorcycleRentChangeEndDateResponse>> Handle(MotorcycleRentChangeEndDateRequest request,
        CancellationToken cancellationToken)
    {
        var rental = await _motorcycleRentalPlanRepository.GetById(request.Id, cancellationToken);
        var rentalPlan = await _rentalPlanRepository.GetById(rental.RentalPlanId, cancellationToken);

        rental.Checkout(rentalPlan, request.ActualReturnDate);

        return Result.Success(new MotorcycleRentChangeEndDateResponse(
            rental.Id,
            rental.MotorcycleId,
            rental.DelivererId,
            rental.RentalPlanId,
            rental.StartDate,
            rental.EndDate,
            rental.ExpectedFinishDate,
            rental.CreatedAt,
            rental.AmountPredicted,
            rental.AmountPaid));
    }
}