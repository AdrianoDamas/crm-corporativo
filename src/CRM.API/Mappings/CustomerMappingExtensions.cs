namespace CRM.API.Mappings;

using CRM.API.Contracts.Requests;
using CRM.API.Contracts.Responses;
using CRM.Domain.Entities;
using CRM.Domain.ValueObjects;

public static class CustomerMappingExtensions
{
    public static CustomerResponse ToResponse(this Customer customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.Name,
            customer.Type,
            customer.Document?.Value ?? string.Empty,
            customer.Document?.Type ?? DocumentType.Cpf,
            customer.Email?.Value ?? string.Empty,
            customer.Phone?.Value,
            customer.Address.ToResponse(),
            customer.BirthDate,
            customer.IsActive,
            customer.StateRegistration,
            customer.IsStateRegistrationExempt,
            customer.CreatedAt,
            customer.UpdatedAt);
    }

    public static AddressResponse? ToResponse(this Address? address)
    {
        if (address is null)
            return null;

        return new AddressResponse(
            address.ZipCode,
            address.Street,
            address.Number,
            address.Neighborhood,
            address.City,
            address.State,
            address.Complement);
    }

    public static Address? ToValueObject(this AddressRequest? request)
    {
        if (request is null)
            return null;

        return Address.Create(
            request.Street,
            request.Number,
            request.Neighborhood,
            request.City,
            request.State,
            request.ZipCode,
            request.Complement);
    }

    public static PhoneNumber? ToValueObject(this string? phone)
    {
        return string.IsNullOrWhiteSpace(phone) ? null : PhoneNumber.Create(phone);
    }
}
