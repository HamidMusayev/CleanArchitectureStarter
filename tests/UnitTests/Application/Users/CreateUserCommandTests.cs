using Application.Abstractions.Persistence;
using Application.Features.Users.Commands.CreateUser;
using AutoMapper;
using Contracts.v1.Users;
using Domain.Repositories;
using Domain.Users;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace UnitTests.Application.Users;

// Verifies behavior & mapping.
public class CreateUserCommandTests
{
    [Fact]
    public async Task Creates_User_And_Returns_Dto()
    {
        var repo = Substitute.For<IUserRepository>();
        var uow = Substitute.For<IUnitOfWork>();

        var loggerFactory = LoggerFactory.Create(_ =>
        {
            /* no providers needed for tests */
        });

        var mapper = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserResponse>()
                    .ForCtorParam("FullName", o => o.MapFrom(s => s.FullName)),
            loggerFactory
        ).CreateMapper();

        var handler = new CreateUserCommandHandler(repo, uow, mapper);
        var result = await handler.Handle(new CreateUserCommand("a@b.com", "Ada", "Lovelace"), default);

        Assert.True(result.IsSuccess);
        await uow.Received().SaveChangesAsync(CancellationToken.None);
    }
}