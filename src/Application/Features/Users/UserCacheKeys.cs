namespace Application.Features.Users;

internal static class UserCacheKeys
{
    public static string ById(Guid id) => $"user:{id}";
}
