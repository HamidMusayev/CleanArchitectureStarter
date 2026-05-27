namespace Contracts.v1.Auth;

public sealed record AuthResponse(string AccessToken, DateTime ExpiresAt, Guid UserId, string Email);
