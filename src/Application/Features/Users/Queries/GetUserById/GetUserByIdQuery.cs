using Contracts.v1.Users;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<UserResponse?>;