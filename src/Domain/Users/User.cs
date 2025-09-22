using Domain.Common;
using Domain.Users.Events;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public sealed class User : AggregateRoot
{
    public Email Email { get; private set; }
    public string GivenName { get; private set; }
    public string FamilyName { get; private set; }
    public string FullName => $"{GivenName} {FamilyName}".Trim();
    public DateTime CreatedAt { get; private set; }

    private User(Email email, string given, string family)
    {
        Email = email; GivenName = given; FamilyName = family; CreatedAt = DateTime.UtcNow;
        Raise(new UserCreatedDomainEvent(Id, Email.Value, CreatedAt));
    }

    public static User Create(string email, string given, string family) =>
        new(Email.Create(email), given, family);
}