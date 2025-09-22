namespace Contracts.v1.Users;

public sealed record CreateUserRequest(string Email, string GivenName, string FamilyName);