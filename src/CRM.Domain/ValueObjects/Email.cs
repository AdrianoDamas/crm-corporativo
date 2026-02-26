using System;

namespace CRM.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    public string Value { get; private set; } = string.Empty;

    private Email()
    {
    }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email is required.", nameof(value));

        var normalized = value.Trim().ToLowerInvariant();
        if (!IsValid(normalized))
            throw new ArgumentException("Email format is invalid.", nameof(value));

        return new Email(normalized);
    }

    private static bool IsValid(string value)
    {
        var atIndex = value.IndexOf('@');
        if (atIndex <= 0 || atIndex == value.Length - 1)
            return false;

        var dotIndex = value.LastIndexOf('.');
        return dotIndex > atIndex + 1 && dotIndex < value.Length - 1;
    }

    public bool Equals(Email? other)
    {
        if (other is null)
            return false;

        return Value == other.Value;
    }

    public override bool Equals(object? obj) => obj is Email other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

    public override string ToString() => Value;
}
