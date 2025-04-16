using Domain.Abstractions;
using Domain.Entities.Users.ValueObjects;

namespace Domain.Entities.Users;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

public class User : Entity<UserId>
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public EUserRole Role { get; set; } = EUserRole.User;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public static User Create()
    {
        var user = new User
        {
            Id = UserId.From(Guid.NewGuid())
        };
        
        return user;
    }
}

public enum EUserRole { Admin, User }