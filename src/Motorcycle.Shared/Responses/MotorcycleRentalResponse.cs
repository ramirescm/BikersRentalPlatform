namespace Motorcycle.Shared.Responses;

public class MotorcycleRentalResponse(
    Guid Id,
    Guid MotorcycleId,
    Guid DelivererId,
    Guid RentalPlanId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    DateTimeOffset ExpectedFinishDate,
    DateTimeOffset CreatedAt,
    decimal AmountPredicted);