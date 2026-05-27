using Application.Abstractions;
using Application.Common.Mappings;
using Application.Common.Results;
using Contracts.v1.Users;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(IUserRepository repo, ICacheService cache)
    : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken ct)
    {
        var key = UserCacheKeys.ById(query.Id);

        var cached = await cache.GetAsync<UserResponse>(key, ct);
        if (cached is not null)
            return Result<UserResponse>.Success(cached);

        var user = await repo.GetByIdAsync(query.Id, ct);
        if (user is null)
            return Result<UserResponse>.NotFound("user.not_found", $"User {query.Id} was not found.");

        var response = user.ToResponse();
        await cache.SetAsync(key, response, CacheTtl, ct);
        return Result<UserResponse>.Success(response);
    }
}
