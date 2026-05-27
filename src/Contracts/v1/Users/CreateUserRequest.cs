namespace Contracts.v1.Users;

public sealed record CreateUserRequest(string Email, string Password, string GivenName, string FamilyName);
