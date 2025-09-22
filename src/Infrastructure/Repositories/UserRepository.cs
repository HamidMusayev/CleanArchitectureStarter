using Domain.Repositories;
using Domain.Users;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly MyAppDbContext _db;
    public UserRepository(MyAppDbContext db) => _db = db;

    public void Add(User user) => _db.Users.Add(user);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
}