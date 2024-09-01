using FluentValidation;

namespace Motorcycle.Shared.Requests;

public class MotorcycleCreateRequestValidator : AbstractValidator<MotorcycleCreateRequest>
{
    public MotorcycleCreateRequestValidator()
    {
        RuleFor(x => x.Year)
            .InclusiveBetween(1500, DateTime.Now.Year) // Validates that the year is realistic
            .WithMessage("Year must be between {FromValue} and the {ToValue}.");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .MaximumLength(100)
            .WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");

        RuleFor(x => x.LicensePlate)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .Matches(@"^[A-Z0-9-]+$") // Adjust regex based on your license plate format requirements
            .WithMessage("{PropertyName} must only contain uppercase letters, numbers, and dashes.")
            .MaximumLength(10)
            .WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");
    }
}