using Application.Data.Abstractions;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Support.Cqrs.Abstractions;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.Users.Commands;

public class RegisterUserHandler(IRepositoryFactory repositoryFactory) :
    ICommandHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IUsersRepository _usersRepository = repositoryFactory.CreateUsersRepository();
    
    public async Task<RegisterUserResult?> HandleAsync(RegisterUserCommand command)
    {
        if (await _usersRepository.ExistsAsync(command.Username))
            return null;
        
        var user = User.Create();
        
        var hashedPassword = new PasswordHasher<User>().HashPassword(user, command.Password);
        
        user.Username = command.Username;
        user.PasswordHash = hashedPassword;
        user.Role = EUserRole.User;

        return new RegisterUserResult { User = await _usersRepository.AddUserAsync(user) };
    }
}

public record RegisterUserCommand : ICommand<RegisterUserResult>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public record RegisterUserResult
{
    public User? User { get; set; }
}