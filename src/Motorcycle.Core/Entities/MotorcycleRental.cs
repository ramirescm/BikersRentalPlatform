namespace Motorcycle.Core.Entities;

public class MotorcycleRental
{
    public Guid Id { get; set; }
    public Guid MotorcycleId { get; set; }
    public Guid DelivererId { get; set; }
    public Guid RentalPlanId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public DateTimeOffset ExpectedFinishDate { get; set; }
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
    public decimal AmountPredicted { get; set; }
    public decimal AmountPaid { get; set; }

    public void CalculateRentalCost(RentalPlan plan)
    {
        StartDate = CreatedAt.AddDays(1);
        EndDate = StartDate.AddDays(plan.Days);
        ExpectedFinishDate = EndDate;

        var planDays = plan.Days;
        var dailyCost = plan.Amount;
        var penaltyRate = plan.Fee;

        var rentalDuration = (EndDate - StartDate).Days + 1;
        var totalCost = dailyCost * rentalDuration;
        AmountPredicted = totalCost;
    }

    public void Checkout(RentalPlan plan, DateTimeOffset actualReturnDate)
    {
        var dailyCost = plan.Amount;
        var penaltyRate = plan.Fee;

        var totalCost = AmountPredicted;

        // Penalty for early return
        if (actualReturnDate < ExpectedFinishDate)
        {
            var earlyReturnDays = (ExpectedFinishDate - actualReturnDate).Days;
            var penaltyAmount = dailyCost * earlyReturnDays * penaltyRate;
            totalCost += penaltyAmount;
        }
        // Additional charge for late return
        else if (actualReturnDate > ExpectedFinishDate)
        {
            var lateReturnDays = (actualReturnDate - ExpectedFinishDate).Days;
            var additionalCharge = 50m * lateReturnDays;
            totalCost += additionalCharge;
        }

        AmountPaid = totalCost;
    }
}