using MediatR;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.UoW;
using Motorcycle.Shared.Requests;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Core.Handlers;

public class
    MotorcycleRentalGetAllRequestHandler : IRequestHandler<MotorcycleRentalGetAllRequest,
    Result<MotorcycleRentalGetAllResponse>>
{
    private readonly IMotorcycleRentalPlanRepository _motorcycleRentalPlanRepository;
    private readonly IUnitOfWork _uow;

    public MotorcycleRentalGetAllRequestHandler(IUnitOfWork uow,
        IMotorcycleRentalPlanRepository motorcycleRentalPlanRepository)
    {
        _uow = uow;
        _motorcycleRentalPlanRepository = motorcycleRentalPlanRepository;
    }

    public async Task<Result<MotorcycleRentalGetAllResponse>> Handle(MotorcycleRentalGetAllRequest request,
        CancellationToken cancellationToken)
    {
        var motorcycles = await _motorcycleRentalPlanRepository.GetAll(cancellationToken);
        return Result.Success(new MotorcycleRentalGetAllResponse(motorcycles));
    }
}