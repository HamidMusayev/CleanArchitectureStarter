using Application.Abstractions;
using Application.Common.Results;
using Contracts.v1.Auth;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository repo,
    IPasswordHasher passwordHasher,
    IJwtTokenService tokenService) : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private static readonly Error InvalidCredentials =
        new("auth.invalid_credentials", "The email or password is incorrect.");

    public async Task<Result<AuthResponse>> Handle(LoginCommand cmd, CancellationToken ct)
    {
        var user = await repo.GetByEmailAsync(cmd.Email, ct);
        if (user is null)
            return Result<AuthResponse>.Unauthorized(InvalidCredentials.Code, InvalidCredentials.Description);

        if (!passwordHasher.Verify(cmd.Password, user.PasswordHash))
            return Result<AuthResponse>.Unauthorized(InvalidCredentials.Code, InvalidCredentials.Description);

        var (token, expiresAt) = tokenService.GenerateToken(user.Id, user.Email.Value);
        return Result<AuthResponse>.Success(new AuthResponse(token, expiresAt, user.Id, user.Email.Value));
    }
}
