using Domain.Users;

namespace Domain.Repositories;

public interface IUserRepository
{
    void Add(User user);
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
}