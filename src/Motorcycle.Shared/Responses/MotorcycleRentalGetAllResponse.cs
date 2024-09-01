using Motorcycle.Shared.Dtos;

namespace Motorcycle.Shared.Responses;

public record MotorcycleRentalGetAllResponse(List<MotorcycleRentalDto> Rentals);