using System;

namespace CRM.Domain.ValueObjects;

public sealed class Address : IEquatable<Address>
{
    public string Street { get; private set; } = string.Empty;
    public string Number { get; private set; } = string.Empty;
    public string? Complement { get; private set; }
    public string Neighborhood { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;

    private Address()
    {
    }

    private Address(string street, string number, string? complement, string neighborhood, string city, string state, string zipCode)
    {
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public static Address Create(string street, string number, string neighborhood, string city, string state, string zipCode, string? complement = null)
    {
        return new Address(
            EnsureValue(street, nameof(street)),
            EnsureValue(number, nameof(number)),
            complement?.Trim(),
            EnsureValue(neighborhood, nameof(neighborhood)),
            EnsureValue(city, nameof(city)),
            EnsureValue(state, nameof(state)),
            NormalizeZip(zipCode));
    }

    private static string EnsureValue(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{propertyName} is required.", propertyName);

        return value.Trim();
    }

    private static string NormalizeZip(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ZipCode is required.", nameof(value));

        var normalized = value.Trim().Replace("-", string.Empty).Replace(" ", string.Empty);
        if (normalized.Length < 5)
            throw new ArgumentException("ZipCode is invalid.", nameof(value));

        return normalized;
    }

    public bool Equals(Address? other)
    {
        if (other is null)
            return false;

        return Street == other.Street
            && Number == other.Number
            && Complement == other.Complement
            && Neighborhood == other.Neighborhood
            && City == other.City
            && State == other.State
            && ZipCode == other.ZipCode;
    }

    public override bool Equals(object? obj) => obj is Address other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Street, Number, Complement, Neighborhood, City, State, ZipCode);

    public override string ToString() => $"{Street}, {Number} - {City}/{State}";
}
