using MediatR;
using Motorcycle.Core.Entities;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.UoW;
using Motorcycle.Shared.Exceptions;
using Motorcycle.Shared.Requests;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Core.Handlers;

public sealed class
    MotorcycleRentalRequestHandler : IRequestHandler<MotorcycleRentalRequest, Result<MotorcycleRentalResponse>>
{
    private readonly IDelivererRepository _delivererRepository;
    private readonly IMotorcycleRentalPlanRepository _motorcycleRentalPlanRepository;
    private readonly IRentalPlanRepository _rentalPlanRepository;
    private readonly IUnitOfWork _uow;

    public MotorcycleRentalRequestHandler(
        IUnitOfWork uow,
        IMotorcycleRentalPlanRepository motorcycleRentalPlanRepository,
        IDelivererRepository delivererRepository,
        IRentalPlanRepository rentalPlanRepository)
    {
        _uow = uow;
        _motorcycleRentalPlanRepository = motorcycleRentalPlanRepository;
        _delivererRepository = delivererRepository;
        _rentalPlanRepository = rentalPlanRepository;
    }

    public async Task<Result<MotorcycleRentalResponse>> Handle(MotorcycleRentalRequest planRequest,
        CancellationToken cancellationToken)
    {
        var motorycycleHasPermission =
            await _delivererRepository.CnhAllowedExistsAsync(planRequest.DelivererId, cancellationToken);
        if (!motorycycleHasPermission)
            return Result.Error<MotorcycleRentalResponse>(
                new BadRequestException("Only delivery drivers qualified in category A can make a rental"));

        var rentalPlan = await _rentalPlanRepository.GetById(planRequest.RentalPlanId, cancellationToken);

        var rental = new MotorcycleRental
        {
            RentalPlanId = planRequest.RentalPlanId,
            MotorcycleId = planRequest.MotorcycleId,
            DelivererId = planRequest.DelivererId
        };

        rental.CalculateRentalCost(rentalPlan);

        await _motorcycleRentalPlanRepository.CreateRental(rental, cancellationToken);
        await _uow.CommitAsync(cancellationToken);

        return Result.Success(new MotorcycleRentalResponse(
            rental.Id,
            rental.MotorcycleId,
            rental.DelivererId,
            rental.RentalPlanId,
            rental.StartDate,
            rental.EndDate,
            rental.ExpectedFinishDate,
            rental.CreatedAt,
            rental.AmountPredicted));
    }
}