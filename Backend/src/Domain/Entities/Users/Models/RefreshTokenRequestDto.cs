using Domain.Entities.Users.ValueObjects;

namespace Domain.Entities.Users.Models;

public class RefreshTokenRequestDto
{
    public required string UserId { get; set; }
    public required string RefreshToken { get; set; }
}