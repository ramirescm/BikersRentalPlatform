using MediatR;
using Microsoft.AspNetCore.Http;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.UoW;
using Motorcycle.Shared.Exceptions;
using Motorcycle.Shared.Requests;
using Motorcycle.Shared.Responses;
using OperationResult;

namespace Motorcycle.Core.Handlers;

public sealed class
    DelivererUpdateCnhRequestHandler : IRequestHandler<DeliveryUploadPhotoCnhRequest,
    Result<DeliveryUploadPhotoCnhResponse>>
{
    private readonly IDelivererRepository _delivererRepository;
    private readonly IUnitOfWork _uow;

    public DelivererUpdateCnhRequestHandler(IDelivererRepository delivererRepository, IUnitOfWork uow)
    {
        _delivererRepository = delivererRepository;
        _uow = uow;
    }

    public async Task<Result<DeliveryUploadPhotoCnhResponse>> Handle(DeliveryUploadPhotoCnhRequest request,
        CancellationToken cancellationToken)
    {
        var (id, file) = request;
        var deliverer = await _delivererRepository.GetByIdAsync(id, cancellationToken);

        if (!IsValidFileExtension(file))
            return Result.Error<DeliveryUploadPhotoCnhResponse>(
                new BadRequestException("File extension invalid. File extension must be 'png' or 'bmp'"));

        var uploadFolder = CreateUploadFolderIfNotExists();

        var fileName = Path.GetFileName(file.FileName);
        var filePath = Path.Combine(uploadFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        await _delivererRepository.UpdateImageCnhAsync(id, filePath);
        return Result.Success(new DeliveryUploadPhotoCnhResponse(id, filePath));
    }

    private bool IsValidFileExtension(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return extension is ".png" or ".bmp";
    }

    private string CreateUploadFolderIfNotExists()
    {
        var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "../../data/images/cnh");
        if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

        return uploadFolder;
    }
}