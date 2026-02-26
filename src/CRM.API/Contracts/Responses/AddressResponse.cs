namespace CRM.API.Contracts.Responses;

public sealed record AddressResponse(
    string ZipCode,
    string Street,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string? Complement);
