namespace CRM.API.Contracts.Requests;

public sealed class UpdateCustomerRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? StateRegistration { get; set; }
    public bool? IsStateRegistrationExempt { get; set; }
    public AddressRequest? Address { get; set; }
}
