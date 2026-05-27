using Domain.Common;
using Domain.Users.Events;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public sealed class User : AggregateRoot, ISoftDeletable
{
    private User()
    {
        Email = null!;
        GivenName = null!;
        FamilyName = null!;
        PasswordHash = null!;
    } // Required by EF Core

    private User(Email email, string given, string family, string passwordHash)
    {
        Email = email;
        GivenName = given;
        FamilyName = family;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        Raise(new UserCreatedDomainEvent(Id, Email.Value, CreatedAt));
    }

    public Email Email { get; private set; }
    public string GivenName { get; private set; }
    public string FamilyName { get; private set; }
    public string PasswordHash { get; private set; }
    public string FullName => $"{GivenName} {FamilyName}".Trim();
    public DateTime CreatedAt { get; private set; }

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }

    public static User Create(string email, string given, string family, string passwordHash)
        => new(Email.Create(email), given, family, passwordHash);

    public void Rename(string givenName, string familyName)
    {
        GivenName = givenName;
        FamilyName = familyName;
        Raise(new UserUpdatedDomainEvent(Id, DateTime.UtcNow));
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        Raise(new UserUpdatedDomainEvent(Id, DateTime.UtcNow));
    }

    public void MarkDeleted(string? deletedBy)
    {
        if (IsDeleted) return;
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
        Raise(new UserDeletedDomainEvent(Id, DeletedAt.Value));
    }
}
