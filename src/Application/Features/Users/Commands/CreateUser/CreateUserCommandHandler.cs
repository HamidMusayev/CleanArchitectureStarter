using Application.Abstractions.Persistence;
using Application.Common.Mappings;
using Application.Common.Results;
using Contracts.v1.Users;
using Domain.Repositories;
using Domain.Users;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateUserCommandHandler(IUserRepository repo, IUnitOfWork uow)
    {
        (_repo, _uow) = (repo, uow);
    }

    public async Task<Result<UserResponse>> Handle(CreateUserCommand cmd, CancellationToken ct)
    {
        var user = User.Create(cmd.Email, cmd.GivenName, cmd.FamilyName);
        _repo.Add(user);
        await _uow.SaveChangesAsync(ct);
        return Result<UserResponse>.Success(user.ToResponse());
    }
}