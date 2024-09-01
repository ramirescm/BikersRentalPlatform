using Motorcycle.Shared.Models;

namespace Motorcycle.Shared.Responses;

public record RentalPlanGetAllResponse(List<RentalPlanDto> Plans);