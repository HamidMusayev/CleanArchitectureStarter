using Domain.Repositories;
using Domain.Users;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class UserRepository(MyAppDbContext db) : IUserRepository
{
    public void Add(User user)
    {
        db.Users.Add(user);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
    }
}