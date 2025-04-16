using Application.Data.Abstractions;
using Application.Services.Abstractions;
using Domain.Entities.Users;
using Domain.Entities.Users.Models;
using Microsoft.AspNetCore.Identity;
using Support.Cqrs.Abstractions;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.Users.Commands;

public class LoginUserHandler(IRepositoryFactory repositoryFactory, IServiceFactory serviceFactory) :
    ICommandHandler<LoginUserCommand, LoginUserResult>
{
    private readonly IUsersRepository _usersRepository = repositoryFactory.CreateUsersRepository();
    private readonly ITokenService _tokenService = serviceFactory.CreateTokenService();

    public async Task<LoginUserResult?> HandleAsync(LoginUserCommand command)
    {
        var user = await _usersRepository.GetByUsernameAsync(command.Username);

        if (user is null)
            return null;

        var passwordVerificationResult =
            new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, command.Password);

        return passwordVerificationResult == PasswordVerificationResult.Failed
            ? null
            : new LoginUserResult { TokenResponse = await _tokenService.CreateTokenResponse(user) };
    }
}

public record LoginUserCommand : ICommand<LoginUserResult>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public record LoginUserResult
{
    public required TokenResponseDto TokenResponse { get; set; }
}