namespace Application.Abstractions;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(Guid userId, string email);
}
