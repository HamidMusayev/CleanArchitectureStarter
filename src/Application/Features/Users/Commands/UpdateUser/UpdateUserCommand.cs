using Application.Common.Results;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(Guid Id, string GivenName, string FamilyName)
    : IRequest<Result>;
