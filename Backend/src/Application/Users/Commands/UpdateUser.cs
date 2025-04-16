using Application.Data.Abstractions;
using Domain.Entities.Users;
using Domain.Entities.Users.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Support.Cqrs.Abstractions;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.Users.Commands;

public class UpdateUserHandler(IRepositoryFactory repositoryFactory)
    : ICommandHandler<UpdateUserCommand, UpdateUserResult>
{
    private readonly IUsersRepository _usersRepository = repositoryFactory.CreateUsersRepository();

    public async Task<UpdateUserResult?> HandleAsync(UpdateUserCommand command)
    {
        var user = await _usersRepository.GetByIdAsync(command.UserId);

        if (user == null)
            return null;

        if (!string.IsNullOrWhiteSpace(command.Password))
        {
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(user, command.Password);

            user.PasswordHash = hashedPassword;
        }

        return new UpdateUserResult { User = await _usersRepository.UpdateUserAsync(user) };
    }
}

public record UpdateUserCommand : ICommand<UpdateUserResult>
{
    public required UserId UserId { get; set; }
    public string Password { get; set; } = string.Empty;
}

public record UpdateUserResult
{
    public User? User { get; set; }
}