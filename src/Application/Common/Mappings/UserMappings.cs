using Contracts.v1.Users;
using Domain.Users;

namespace Application.Common.Mappings;

public static class UserMappings
{
    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse(user.Id, user.Email.Value, user.FullName,
            new DateTimeOffset(user.CreatedAt, TimeSpan.Zero));
    }
}