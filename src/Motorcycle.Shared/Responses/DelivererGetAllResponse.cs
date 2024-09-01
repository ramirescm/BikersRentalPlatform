using Motorcycle.Shared.Dtos;

namespace Motorcycle.Shared.Responses;

public record DelivererGetAllResponse(List<DelivererDto> Deliverers);