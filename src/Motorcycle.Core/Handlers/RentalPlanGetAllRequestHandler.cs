using MediatR;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.UoW;
using Motorcycle.Shared.Requests;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Core.Handlers;

public sealed class
    RentalPlanGetAllRequestHandler : IRequestHandler<RentalPlanGetAllRequest, Result<RentalPlanGetAllResponse>>
{
    private readonly IRentalPlanRepository _rentalPlanRepository;
    private readonly IUnitOfWork _uow;

    public RentalPlanGetAllRequestHandler(IUnitOfWork uow, IRentalPlanRepository rentalPlanRepository)
    {
        _uow = uow;
        _rentalPlanRepository = rentalPlanRepository;
    }

    public async Task<Result<RentalPlanGetAllResponse>> Handle(RentalPlanGetAllRequest request,
        CancellationToken cancellationToken)
    {
        var plans = await _rentalPlanRepository.GetAll(cancellationToken);
        return Result.Success(new RentalPlanGetAllResponse(plans));
    }
}