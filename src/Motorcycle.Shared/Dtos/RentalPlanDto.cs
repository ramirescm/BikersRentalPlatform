namespace Motorcycle.Shared.Models;

public class RentalPlanDto
{
    public Guid Id { get; set; }
    public int Days { get; set; }
    public decimal Amount { get; set; }
    public decimal Fee { get; set; }
}