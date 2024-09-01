using FluentValidation;

namespace Motorcycle.Shared.Requests;

public sealed class DelivererCreateRequestValidator : AbstractValidator<DelivererCreateRequest>
{
    public DelivererCreateRequestValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be at most 100 characters long.");

        RuleFor(d => d.Cnpj)
            .NotEmpty().WithMessage("CNPJ is required.")
            .Length(14).WithMessage("CNPJ must be exactly 14 characters long.")
            .Matches(@"^\d{14}$").WithMessage("CNPJ must contain only numbers.");

        RuleFor(d => d.BirthDate)
            .Must(BeAValidDate)
            .WithMessage("Birth date is not a valid date format.")
            .Must(BeNotInTheFuture)
            .WithMessage("Birth date cannot be in the future.");

        RuleFor(d => d.CnhNumber)
            .NotEmpty().WithMessage("CNH Number is required.")
            .Length(11).WithMessage("CNH Number must be exactly 11 characters long.")
            .Matches(@"^\d{11}$").WithMessage("CNH Number must contain only numbers.");

        RuleFor(d => d.CnhType)
            .NotEmpty().WithMessage("CNH Type is required.")
            .Must(type => type == "A" || type == "B" || type == "A+B")
            .WithMessage("CNH Type must be 'A', 'B', or 'A+B'.");

        RuleFor(d => d.CnhPathImage)
            .MaximumLength(200).WithMessage("CNH Image path must be at most 200 characters long.")
            .When(d => !string.IsNullOrWhiteSpace(d.CnhPathImage));
    }

    private bool BeAValidDate(string birthDate)
    {
        return DateTime.TryParse(birthDate, out _);
    }

    private bool BeNotInTheFuture(string birthDate)
    {
        if (DateTime.TryParse(birthDate, out var date)) return date <= DateTime.Today;
        return true; // If the date is invalid, fail validation
    }
}