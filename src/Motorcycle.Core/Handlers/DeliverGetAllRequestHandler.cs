using MediatR;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.UoW;
using Motorcycle.Shared.Requests;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Core.Handlers;

public class DeliverGetAllRequestHandler : IRequestHandler<DelivererGetAllRequest, Result<DelivererGetAllResponse>>
{
    private readonly IDelivererRepository _delivererRepository;
    private readonly IUnitOfWork _uow;

    public DeliverGetAllRequestHandler(IUnitOfWork uow, IDelivererRepository delivererRepository)
    {
        _uow = uow;
        _delivererRepository = delivererRepository;
    }

    public async Task<Result<DelivererGetAllResponse>> Handle(DelivererGetAllRequest request,
        CancellationToken cancellationToken)
    {
        var deliverers = await _delivererRepository.GetAll(request.Cnh, cancellationToken);
        return Result.Success(new DelivererGetAllResponse(deliverers));
    }
}