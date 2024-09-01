using FluentValidation;

namespace Motorcycle.Shared.Requests;

public class MotorcycleLicensePlateUpdateRequestValidator : AbstractValidator<MotorcycleLicensePlateUpdateRequest>
{
    public MotorcycleLicensePlateUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.");

        RuleFor(x => x.LicensePlate)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .Matches(@"^[A-Z0-9-]+$") // Adjust regex based on your license plate format requirements
            .WithMessage("{PropertyName} must only contain uppercase letters, numbers, and dashes.")
            .MaximumLength(10)
            .WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");
    }
}