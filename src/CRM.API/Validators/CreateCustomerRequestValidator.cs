namespace CRM.API.Validators;

using CRM.API.Contracts.Requests;
using CRM.Domain.Entities;
using CRM.Domain.ValueObjects;
using FluentValidation;

public sealed class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Document)
            .NotEmpty();

        RuleFor(x => x.DocumentType)
            .IsInEnum();

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

        RuleFor(x => x)
            .Must(request => request.Type switch
            {
                CustomerType.Individual => request.DocumentType == DocumentType.Cpf,
                CustomerType.Company => request.DocumentType == DocumentType.Cnpj,
                _ => true
            })
            .WithMessage("Document type must match the customer type (CPF for Individual, CNPJ for Company).");

        When(x => x.Type == CustomerType.Individual, () =>
        {
            RuleFor(x => x.BirthDate)
                .NotNull().WithMessage("Birth date is required for individuals.")
                .Must(date => date == null || date.Value <= DateTime.UtcNow.AddYears(-18))
                .WithMessage("Customer must be at least 18 years old.");

            RuleFor(x => x.StateRegistration)
                .Empty()
                .WithMessage("State registration applies only to companies.");
        });

        When(x => x.Type == CustomerType.Company, () =>
        {
            RuleFor(x => x.StateRegistration)
                .NotEmpty()
                .When(x => !x.IsStateRegistrationExempt)
                .WithMessage("State registration (IE) is required when the company is not exempt.")
                .MaximumLength(50);
        });
    }
}
