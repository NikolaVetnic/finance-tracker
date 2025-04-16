using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Users.Models;

public class UserUpdateDto
{
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, one number, and one special character.")]
    public string Password { get; set; } = string.Empty;
}