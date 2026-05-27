using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Results;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler(
    IUserRepository repo,
    IUnitOfWork uow,
    ICacheService cache,
    ICurrentUserService currentUser) : IRequestHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(DeleteUserCommand cmd, CancellationToken ct)
    {
        var user = await repo.GetByIdAsync(cmd.Id, ct);
        if (user is null)
            return Result.NotFound("user.not_found", $"User {cmd.Id} was not found.");

        user.MarkDeleted(currentUser.UserId);
        await uow.SaveChangesAsync(ct);
        await cache.RemoveAsync(UserCacheKeys.ById(cmd.Id), ct);
        return Result.Success();
    }
}
