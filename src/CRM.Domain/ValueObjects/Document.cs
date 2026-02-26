using System;
using System.Linq;

namespace CRM.Domain.ValueObjects;

public enum DocumentType
{
    Cpf = 1,
    Cnpj = 2
}

public sealed class Document : IEquatable<Document>
{
    public string Value { get; private set; } = string.Empty;
    public DocumentType Type { get; private set; }

    private Document()
    {
    }

    private Document(string value, DocumentType type)
    {
        Value = value;
        Type = type;
    }

    public static Document FromCpf(string value) => new Document(Normalize(value, 11, "CPF"), DocumentType.Cpf);

    public static Document FromCnpj(string value) => new Document(Normalize(value, 14, "CNPJ"), DocumentType.Cnpj);

    public static Document Create(string value, DocumentType type) => type switch
    {
        DocumentType.Cpf => FromCpf(value),
        DocumentType.Cnpj => FromCnpj(value),
        _ => throw new ArgumentOutOfRangeException(nameof(type), "Unsupported document type.")
    };

    private static string Normalize(string value, int requiredDigits, string label)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{label} is required.", nameof(value));

        var digits = new string(value.Where(char.IsDigit).ToArray());
        if (digits.Length != requiredDigits)
            throw new ArgumentException($"{label} must contain {requiredDigits} digits.", nameof(value));

        return digits;
    }

    public bool Equals(Document? other)
    {
        if (other is null)
            return false;

        return Value == other.Value && Type == other.Type;
    }

    public override bool Equals(object? obj) => obj is Document other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Value, Type);

    public override string ToString() => Value;
}
