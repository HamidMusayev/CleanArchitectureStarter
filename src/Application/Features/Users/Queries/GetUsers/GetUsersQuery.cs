using Application.Common.Results;
using Contracts.Common;
using Contracts.v1.Users;
using MediatR;

namespace Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery(int Page = 1, int PageSize = 50)
    : IRequest<Result<PageResult<UserResponse>>>;
