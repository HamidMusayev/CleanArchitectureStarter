using Application.Abstractions.Persistence;
using Application.Features.Users.Commands.CreateUser;
using Domain.Repositories;
using NSubstitute;

namespace UnitTests.Application.Users;

public class CreateUserCommandTests
{
    [Fact]
    public async Task Creates_User_And_Returns_Dto()
    {
        var repo = Substitute.For<IUserRepository>();
        var uow = Substitute.For<IUnitOfWork>();

        var handler = new CreateUserCommandHandler(repo, uow);
        var result = await handler.Handle(new CreateUserCommand("a@b.com", "Ada", "Lovelace"), default);

        Assert.True(result.IsSuccess);
        Assert.Equal("Ada Lovelace", result.Value!.FullName);
        Assert.Equal("a@b.com", result.Value.Email);
        await uow.Received().SaveChangesAsync(CancellationToken.None);
    }
}