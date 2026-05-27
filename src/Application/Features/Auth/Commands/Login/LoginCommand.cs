using Application.Common.Results;
using Contracts.v1.Auth;
using MediatR;

namespace Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponse>>;
