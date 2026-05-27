using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Results;
using Application.Features.Users.Commands.CreateUser;
using Domain.Repositories;
using Domain.Users;
using NSubstitute;

namespace UnitTests.Application.Users;

public class CreateUserCommandTests
{
    [Fact]
    public async Task Creates_User_And_Returns_Dto()
    {
        var repo = Substitute.For<IUserRepository>();
        var uow = Substitute.For<IUnitOfWork>();
        var hasher = Substitute.For<IPasswordHasher>();
        hasher.Hash(Arg.Any<string>()).Returns("hashed");
        repo.GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((User?)null);

        var handler = new CreateUserCommandHandler(repo, uow, hasher);
        var result = await handler.Handle(
            new CreateUserCommand("a@b.com", "password123", "Ada", "Lovelace"), default);

        Assert.True(result.IsSuccess);
        Assert.Equal("Ada Lovelace", result.Value!.FullName);
        Assert.Equal("a@b.com", result.Value.Email);
        await uow.Received().SaveChangesAsync(CancellationToken.None);
        hasher.Received().Hash("password123");
    }

    [Fact]
    public async Task Returns_Conflict_When_Email_Already_Exists()
    {
        var repo = Substitute.For<IUserRepository>();
        var uow = Substitute.For<IUnitOfWork>();
        var hasher = Substitute.For<IPasswordHasher>();
        var existing = User.Create("a@b.com", "Ada", "Lovelace", "hash");
        repo.GetByEmailAsync("a@b.com", Arg.Any<CancellationToken>()).Returns(existing);

        var handler = new CreateUserCommandHandler(repo, uow, hasher);
        var result = await handler.Handle(
            new CreateUserCommand("a@b.com", "password123", "Ada", "Lovelace"), default);

        Assert.True(result.IsFailure);
        Assert.Equal(ResultStatus.Conflict, result.Status);
        await uow.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
