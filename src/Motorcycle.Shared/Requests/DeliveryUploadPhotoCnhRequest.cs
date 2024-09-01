using MediatR;
using Microsoft.AspNetCore.Http;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Shared.Requests;

public record DeliveryUploadPhotoCnhRequest(Guid Id, IFormFile File) : IRequest<Result<DeliveryUploadPhotoCnhResponse>>;