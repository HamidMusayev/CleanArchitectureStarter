using Domain.Users;
using Xunit;

namespace UnitTests.Domain.Users;

// Ensures invariants/events.
public class UserTests
{
    [Fact]
    public void Create_Raises_CreatedEvent()
    {
        var user = User.Create("a@b.com", "Ada", "Lovelace");
        Assert.NotEqual(default, user.Id);
        Assert.Equal("Ada Lovelace", user.FullName);
    }
}