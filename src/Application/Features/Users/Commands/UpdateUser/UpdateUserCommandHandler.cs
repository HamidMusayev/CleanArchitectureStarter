using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Results;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(
    IUserRepository repo,
    IUnitOfWork uow,
    ICacheService cache) : IRequestHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle(UpdateUserCommand cmd, CancellationToken ct)
    {
        var user = await repo.GetByIdAsync(cmd.Id, ct);
        if (user is null)
            return Result.NotFound("user.not_found", $"User {cmd.Id} was not found.");

        user.Rename(cmd.GivenName, cmd.FamilyName);
        await uow.SaveChangesAsync(ct);
        await cache.RemoveAsync(UserCacheKeys.ById(cmd.Id), ct);
        return Result.Success();
    }
}
