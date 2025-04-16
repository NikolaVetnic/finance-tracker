using Domain.Entities.Users;
using Domain.Entities.Users.ValueObjects;

namespace Application.Data.Abstractions;

public interface IUsersRepository
{
    Task<User?> GetByIdAsync(UserId userId);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ExistsAsync(string username);
    Task<User?> AddUserAsync(User user);
    Task<User?> UpdateUserAsync(User user);
}