using System.Text.RegularExpressions;
using Domain.Common;

namespace Domain.Users.ValueObjects;

public sealed class Email : ValueObject
{
    private static readonly Regex Rx = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    private Email(string value)
    {
        Value = value.ToLowerInvariant();
    }

    public string Value { get; }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Rx.IsMatch(value))
            throw new ArgumentException("Invalid email.", nameof(value));
        return new Email(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
}