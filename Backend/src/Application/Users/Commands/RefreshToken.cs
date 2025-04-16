using Application.Data.Abstractions;
using Application.Services.Abstractions;
using Domain.Entities.Users;
using Domain.Entities.Users.Models;
using Domain.Entities.Users.ValueObjects;
using Support.Cqrs.Abstractions;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Application.Users.Commands;

public class RefreshTokenHandler(IRepositoryFactory repositoryFactory, IServiceFactory serviceFactory) :
    ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IUsersRepository _usersRepository = repositoryFactory.CreateUsersRepository();
    private readonly ITokenService _tokenService = serviceFactory.CreateTokenService();

    public async Task<RefreshTokenResult?> HandleAsync(RefreshTokenCommand command)
    {
        var user = await ValidateRefreshTokenAsync(command.UserId, command.RefreshToken);

        return user is null
            ? null
            : new RefreshTokenResult { TokenResponse = await _tokenService.CreateTokenResponse(user) };
    }

    private async Task<User?> ValidateRefreshTokenAsync(UserId userId, string refreshToken)
    {
        var user = await _usersRepository.GetByIdAsync(userId);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return null;

        return user;
    }
}

public class RefreshTokenCommand : ICommand<RefreshTokenResult>
{
    public required UserId UserId { get; set; }
    public required string RefreshToken { get; set; }
}

public class RefreshTokenResult
{
    public required TokenResponseDto TokenResponse { get; set; }
}