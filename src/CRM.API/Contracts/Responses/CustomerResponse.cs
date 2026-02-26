namespace CRM.API.Contracts.Responses;

using CRM.Domain.Entities;
using CRM.Domain.ValueObjects;

public sealed record CustomerResponse(
    Guid Id,
    string Name,
    CustomerType Type,
    string Document,
    DocumentType DocumentType,
    string Email,
    string? Phone,
    AddressResponse? Address,
    DateTime? BirthDate,
    bool IsActive,
    string? StateRegistration,
    bool IsStateRegistrationExempt,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
