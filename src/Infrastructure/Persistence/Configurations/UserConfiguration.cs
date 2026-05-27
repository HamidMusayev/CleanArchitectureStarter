using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");
        b.HasKey(x => x.Id);
        b.Property(x => x.CreatedAt).IsRequired();
        b.OwnsOne(x => x.Email,
            e => { e.Property(p => p.Value).HasColumnName("Email").HasMaxLength(256).IsRequired(); });
        b.Ignore(x => x.DomainEvents);
    }
}