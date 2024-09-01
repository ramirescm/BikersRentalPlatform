using Motorcycle.Shared.Models;

namespace Motorcycle.Shared.Responses;

public record MotorcycleGetAllResponse(List<MotorcycleDto> Motorcycles);