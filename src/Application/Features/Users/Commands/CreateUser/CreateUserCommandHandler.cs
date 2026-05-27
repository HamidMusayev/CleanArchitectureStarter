using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Mappings;
using Application.Common.Results;
using Contracts.v1.Users;
using Domain.Repositories;
using Domain.Users;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler(
    IUserRepository repo,
    IUnitOfWork uow,
    IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(CreateUserCommand cmd, CancellationToken ct)
    {
        var existing = await repo.GetByEmailAsync(cmd.Email, ct);
        if (existing is not null)
            return Result<UserResponse>.Conflict("user.email_taken", $"Email '{cmd.Email}' is already in use.");

        var hash = passwordHasher.Hash(cmd.Password);
        var user = User.Create(cmd.Email, cmd.GivenName, cmd.FamilyName, hash);
        repo.Add(user);
        await uow.SaveChangesAsync(ct);
        return Result<UserResponse>.Success(user.ToResponse());
    }
}
