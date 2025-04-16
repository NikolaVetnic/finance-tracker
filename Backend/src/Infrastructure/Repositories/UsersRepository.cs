using Application.Data.Abstractions;
using Domain.Entities.Users;
using Domain.Entities.Users.ValueObjects;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UsersRepository(UsersDbContext context) : IUsersRepository
{
    public async Task<User?> GetByIdAsync(UserId userId)
    {
        return await context.Users.FindAsync(userId);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> ExistsAsync(string username)
    {
        return await context.Users.AnyAsync(u => u.Username == username);
    }
    
    public async Task<User?> AddUserAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        return user;
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
        
        return user;
    }
}