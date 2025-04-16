using Domain.Entities.Users;
using Domain.Entities.Users.Models;

namespace Application.Services.Abstractions;

public interface ITokenService : IServiceBase
{
    Task<TokenResponseDto> CreateTokenResponse(User user);
}