using Application.Common.Results;
using Contracts.v1.Users;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(string Email, string GivenName, string FamilyName)
    : IRequest<Result<UserResponse>>;