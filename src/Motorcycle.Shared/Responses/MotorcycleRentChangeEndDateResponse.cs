namespace Motorcycle.Shared.Responses;

public record MotorcycleRentChangeEndDateResponse(
    Guid Id,
    Guid MotorcycleId,
    Guid DelivererId,
    Guid RentalPlanId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    DateTimeOffset ExpectedFinishDate,
    DateTimeOffset CreatedAt,
    decimal AmountPredicted,
    decimal AmountPaid);