using Domain.Common;
using Domain.Users.Events;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public sealed class User : AggregateRoot
{
    private User()
    {
        Email = null!;
        GivenName = null!;
        FamilyName = null!;
    } // Required by EF Core

    private User(Email email, string given, string family)
    {
        Email = email;
        GivenName = given;
        FamilyName = family;
        CreatedAt = DateTime.UtcNow;
        Raise(new UserCreatedDomainEvent(Id, Email.Value, CreatedAt));
    }

    public Email Email { get; }
    public string GivenName { get; }
    public string FamilyName { get; }
    public string FullName => $"{GivenName} {FamilyName}".Trim();
    public DateTime CreatedAt { get; }

    public static User Create(string email, string given, string family)
    {
        return new User(Email.Create(email), given, family);
    }
}