namespace Motorcycle.Shared.Dtos;

public class MotorcycleRentalDto
{
    public Guid Id { get; set; }
    public Guid MotorcycleId { get; set; }
    public Guid DelivererId { get; set; }
    public Guid RentalPlanId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public DateTimeOffset ExpectedFinishDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public decimal AmountPredicted { get; set; }
    public decimal AmountPaid { get; set; }
}