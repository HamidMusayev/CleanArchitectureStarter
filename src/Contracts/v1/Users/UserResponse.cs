namespace Contracts.v1.Users;

public sealed record UserResponse(Guid Id, string Email, string FullName, DateTimeOffset CreatedAt);