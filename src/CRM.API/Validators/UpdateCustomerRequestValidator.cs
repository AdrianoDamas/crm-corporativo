namespace CRM.API.Validators;

using CRM.API.Contracts.Requests;
using FluentValidation;

public sealed class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200);

        RuleFor(x => x.Phone)
            .Matches("^[0-9\\s\\-\\(\\)\\+]{10,20}$")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("Phone must contain between 10 and 20 digits/symbols.");

        When(x => x.Address is not null, () =>
        {
            RuleFor(x => x.Address!)
                .SetValidator(new AddressRequestValidator());
        });
    }
}
