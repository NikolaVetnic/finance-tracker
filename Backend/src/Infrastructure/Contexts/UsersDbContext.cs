using Domain.Entities.Users;
using Domain.Entities.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity
                .Property(u => u.Id)
                .HasConversion(
                    id => id.Value, 
                    value => UserId.From(value));

            entity
                .Property(u => u.Role)
                .HasConversion<string>();
        });
        
        base.OnModelCreating(builder);
    }
}