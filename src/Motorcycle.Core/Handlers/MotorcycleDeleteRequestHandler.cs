using MediatR;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.UoW;
using Motorcycle.Shared.Exceptions;
using Motorcycle.Shared.Requests;
using OperationResult;

namespace Motorcycle.Core.Handlers;

public sealed class MotorcycleDeleteRequestHandler : IRequestHandler<MotorcycleDeleteRequest, Result>
{
    private readonly IMotorycycleRepository _motorycycleRepository;
    private readonly IRentalPlanRepository _rentalPlanRepository;
    private readonly IUnitOfWork _uow;

    public MotorcycleDeleteRequestHandler(
        IUnitOfWork uow,
        IMotorycycleRepository motorycycleRepository,
        IRentalPlanRepository rentalPlanRepository)
    {
        _uow = uow;
        _motorycycleRepository = motorycycleRepository;
        _rentalPlanRepository = rentalPlanRepository;
    }

    public async Task<Result> Handle(MotorcycleDeleteRequest request, CancellationToken cancellationToken)
    {
        var existsPlanForMotorcycle =
            await _rentalPlanRepository.RentalPlanMotorcycleExistsAsync(request.Id, cancellationToken);
        if (existsPlanForMotorcycle)
            return Result.Error(
                new BadRequestException("You can't remove because, already exists an plan for this motorcycle"));

        await _motorycycleRepository.DeleteById(request.Id, cancellationToken);
        return Result.Success();
    }
}