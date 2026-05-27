using Application.Common.Mappings;
using Application.Common.Results;
using Contracts.Common;
using Contracts.v1.Users;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler(IUserRepository repo)
    : IRequestHandler<GetUsersQuery, Result<PageResult<UserResponse>>>
{
    public async Task<Result<PageResult<UserResponse>>> Handle(GetUsersQuery query, CancellationToken ct)
    {
        var (items, total) = await repo.GetPagedAsync(query.Page, query.PageSize, ct);
        var dtos = items.Select(u => u.ToResponse()).ToList();
        return Result<PageResult<UserResponse>>.Success(
            new PageResult<UserResponse>(dtos, query.Page, query.PageSize, total));
    }
}
