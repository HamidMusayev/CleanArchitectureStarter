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
        b.Property(x => x.GivenName).IsRequired().HasMaxLength(100);
        b.Property(x => x.FamilyName).IsRequired().HasMaxLength(100);
        b.Property(x => x.PasswordHash).IsRequired().HasMaxLength(256);
        b.Property(x => x.IsDeleted).IsRequired();
        b.Property(x => x.DeletedAt);
        b.Property(x => x.DeletedBy).HasMaxLength(256);

        b.OwnsOne(x => x.Email,
            e =>
            {
                e.Property(p => p.Value).HasColumnName("Email").HasMaxLength(256).IsRequired();
                e.HasIndex(p => p.Value).IsUnique();
            });

        b.Ignore(x => x.DomainEvents);

        // Global soft-delete filter: queries automatically exclude deleted rows.
        // Use .IgnoreQueryFilters() to opt out (e.g., admin views).
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}
