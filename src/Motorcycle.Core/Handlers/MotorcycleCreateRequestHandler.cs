using MediatR;
using Motorcycle.Core.Events;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.Services;
using Motorcycle.Core.UoW;
using Motorcycle.Shared.Exceptions;
using Motorcycle.Shared.Requests;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Core.Handlers;

public sealed class MotorcycleCreateRequestHandler : IRequestHandler<MotorcycleCreateRequest, Result<MotorcycleCreateResponse>>
{
    private readonly IMotorycycleRepository _motorycycleRepository;
    private readonly IUnitOfWork _uow;
    private readonly IMessageBus _messageBus;

    public MotorcycleCreateRequestHandler(IUnitOfWork uow, IMotorycycleRepository motorycycleRepository, IMessageBus messageBus)
    {
        _uow = uow;
        _motorycycleRepository = motorycycleRepository;
        _messageBus = messageBus;
    }

    public async Task<Result<MotorcycleCreateResponse>> Handle(MotorcycleCreateRequest request,
        CancellationToken cancellationToken)
    {
        var motorcycleExists =
            await _motorycycleRepository.LicensePlateExistsAsync(request.LicensePlate, cancellationToken);
        if (motorcycleExists)
            return Result.Error<MotorcycleCreateResponse>(new BadRequestException("License Plate already exists"));

        var moto = new Entities.Motorcycle
        {
            Id = Guid.NewGuid(),
            Year = request.Year,
            Model = request.Model,
            LicensePlate = request.LicensePlate
        };
        
        await _motorycycleRepository.Add(moto, cancellationToken);
        await _uow.CommitAsync(cancellationToken);

        var eventRegistered = new MotorcycleRegistered()
        {
            Id = moto.Id,
            Year = moto.Year,
            Model = moto.Model,
            LicensePlate = moto.LicensePlate
        };
        await _messageBus.PublishAsync("motorcycle_events", eventRegistered);

        return Result.Success(new MotorcycleCreateResponse(moto.Id));
    }
}