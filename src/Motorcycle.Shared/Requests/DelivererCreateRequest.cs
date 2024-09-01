using MediatR;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Shared.Requests;

public record DelivererCreateRequest(
    string Email,
    string Name,
    string Cnpj,
    string BirthDate,
    string CnhNumber,
    string CnhType,
    string CnhPathImage) : IRequest<Result<DelivererCreateResponse>>;