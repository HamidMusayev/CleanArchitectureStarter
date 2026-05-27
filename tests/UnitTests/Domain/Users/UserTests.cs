using Domain.Users;

namespace UnitTests.Domain.Users;

public class UserTests
{
    private static User MakeUser() =>
        User.Create("a@b.com", "Ada", "Lovelace", passwordHash: "hash");

    [Fact]
    public void Create_Sets_Identity_And_FullName_And_Raises_Event()
    {
        var user = MakeUser();

        Assert.NotEqual(default, user.Id);
        Assert.Equal("Ada Lovelace", user.FullName);
        Assert.Equal("a@b.com", user.Email.Value);
        Assert.False(user.IsDeleted);
        Assert.Single(user.DomainEvents);
    }

    [Fact]
    public void Rename_Updates_Names_And_Raises_Updated_Event()
    {
        var user = MakeUser();
        user.ClearEvents();

        user.Rename("Grace", "Hopper");

        Assert.Equal("Grace Hopper", user.FullName);
        Assert.Single(user.DomainEvents);
    }

    [Fact]
    public void MarkDeleted_Sets_IsDeleted_And_Raises_Deleted_Event()
    {
        var user = MakeUser();
        user.ClearEvents();

        user.MarkDeleted("admin-id");

        Assert.True(user.IsDeleted);
        Assert.Equal("admin-id", user.DeletedBy);
        Assert.NotNull(user.DeletedAt);
        Assert.Single(user.DomainEvents);
    }

    [Fact]
    public void MarkDeleted_Twice_Is_Idempotent()
    {
        var user = MakeUser();
        user.MarkDeleted("admin-id");
        user.ClearEvents();

        user.MarkDeleted("other");

        Assert.Equal("admin-id", user.DeletedBy);
        Assert.Empty(user.DomainEvents);
    }

    [Fact]
    public void ChangePassword_Updates_Hash()
    {
        var user = MakeUser();

        user.ChangePassword("new-hash");

        Assert.Equal("new-hash", user.PasswordHash);
    }
}
