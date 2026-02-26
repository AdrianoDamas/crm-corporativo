namespace CRM.API.Validators;

using CRM.API.Contracts.Requests;
using FluentValidation;

public sealed class AddressRequestValidator : AbstractValidator<AddressRequest>
{
    public AddressRequestValidator()
    {
        RuleFor(a => a.ZipCode)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(a => a.Street)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(a => a.Number)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(a => a.Neighborhood)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(a => a.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(a => a.State)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(a => a.Complement)
            .MaximumLength(100)
            .When(a => !string.IsNullOrWhiteSpace(a.Complement));
    }
}
