using System.ComponentModel.DataAnnotations;

namespace BaseApiStuff.Models.User;

public class RegisterUserDto
{
    [Required]
    public string UserName { get; set; }
    [Required] [EmailAddress]public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}