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
    DelivererCreateRequestHandler : IRequestHandler<DelivererCreateRequest, Result<DelivererCreateResponse>>
{
    private readonly IDelivererRepository _delivererRepository;
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepository;

    public DelivererCreateRequestHandler(IDelivererRepository delivererRepository, IUnitOfWork uow,
        IUserRepository userRepository)
    {
        _delivererRepository = delivererRepository;
        _uow = uow;
        _userRepository = userRepository;
    }

    public async Task<Result<DelivererCreateResponse>> Handle(DelivererCreateRequest request,
        CancellationToken cancellationToken)
    {
        var delivererCnpjExists = await _delivererRepository.CnpjExistsAsync(request.Cnpj, cancellationToken);
        if (delivererCnpjExists)
            return Result.Error<DelivererCreateResponse>(new BadRequestException("CNPJ already exists"));

        var delivererCnhExists = await _delivererRepository.CnhExistsAsync(request.CnhNumber, cancellationToken);
        if (delivererCnhExists)
            return Result.Error<DelivererCreateResponse>(new BadRequestException("CNH already exists"));

        var id = await _delivererRepository.Add(new Deliverer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Cnpj = request.Cnpj,
            BirthDate = DateOnly.Parse(request.BirthDate),
            CnhNumber = request.CnhNumber,
            CnhType = request.CnhType,
            CnhPathImage = request.CnhPathImage
        }, cancellationToken);

        await _userRepository.Create(
            new User { Email = request.Email, Name = request.Name, Role = "DELIVER", Password = "123456" },
            cancellationToken);
        await _uow.CommitAsync(cancellationToken);

        return Result.Success(new DelivererCreateResponse(id.ToString()));
    }
}