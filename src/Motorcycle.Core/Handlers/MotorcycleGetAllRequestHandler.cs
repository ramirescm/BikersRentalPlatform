using MediatR;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.UoW;
using Motorcycle.Shared.Requests;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Core.Handlers;

public sealed class
    MotorcycleGetAllRequestHandler : IRequestHandler<MotorcycleGetAllRequest, Result<MotorcycleGetAllResponse>>
{
    private readonly IMotorycycleRepository _motorycycleRepository;
    private readonly IUnitOfWork _uow;

    public MotorcycleGetAllRequestHandler(IUnitOfWork uow, IMotorycycleRepository motorycycleRepository)
    {
        _uow = uow;
        _motorycycleRepository = motorycycleRepository;
    }

    public async Task<Result<MotorcycleGetAllResponse>> Handle(MotorcycleGetAllRequest request,
        CancellationToken cancellationToken)
    {
        var motorcycles = await _motorycycleRepository.GetAll(request.LicensePlate, cancellationToken);
        return Result.Success(new MotorcycleGetAllResponse(motorcycles));
    }
}