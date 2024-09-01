using FluentValidation;

namespace Motorcycle.Shared.Requests;

public class MotorcycleRentalRequestValidator : AbstractValidator<MotorcycleRentalRequest>
{
    public MotorcycleRentalRequestValidator()
    {
        RuleFor(x => x.RentalPlanId)
            .NotEmpty();
        RuleFor(x => x.MotorcycleId)
            .NotEmpty();
        RuleFor(x => x.DelivererId)
            .NotEmpty();
    }
}