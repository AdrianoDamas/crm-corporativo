namespace CRM.API.Contracts.Requests;

using CRM.Domain.Entities;
using CRM.Domain.ValueObjects;

public sealed class CreateCustomerRequest
{
    public string Name { get; set; } = string.Empty;
    public CustomerType Type { get; set; }
    public DocumentType DocumentType { get; set; }
    public string Document { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? StateRegistration { get; set; }
    public bool IsStateRegistrationExempt { get; set; }
    public AddressRequest? Address { get; set; }
}
