using MediatR;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.UoW;
using Motorcycle.Shared.Exceptions;
using Motorcycle.Shared.Requests;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Core.Handlers;

public sealed class MotorcycleUpdateLicensePlateRequestHandler : IRequestHandler<MotorcycleLicensePlateUpdateRequest,
    Result<MotorcycleLicensePlateUpdateResponse>>
{
    private readonly IMotorycycleRepository _motorycycleRepository;
    private readonly IUnitOfWork _uow;

    public MotorcycleUpdateLicensePlateRequestHandler(IUnitOfWork uow, IMotorycycleRepository motorycycleRepository)
    {
        _uow = uow;
        _motorycycleRepository = motorycycleRepository;
    }

    public async Task<Result<MotorcycleLicensePlateUpdateResponse>> Handle(MotorcycleLicensePlateUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var motorcycleExists =
            await _motorycycleRepository.LicensePlateExistsAsync(request.LicensePlate, cancellationToken);
        if (motorcycleExists)
            return Result.Error<MotorcycleLicensePlateUpdateResponse>(
                new BadRequestException("License Plate already exists"));

        await _motorycycleRepository.UpdateLicensePlate(request.Id, request.LicensePlate, cancellationToken);
        return Result.Success(new MotorcycleLicensePlateUpdateResponse());
    }
}