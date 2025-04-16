using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Users.Models;

public class UserDto
{
    [Required]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 100 characters.")]
    [EmailAddress(ErrorMessage = "The username must be a valid email address.")]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, one number, and one special character.")]
    public string Password { get; set; } = string.Empty;
}