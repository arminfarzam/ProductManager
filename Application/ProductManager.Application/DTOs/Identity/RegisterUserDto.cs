using System.ComponentModel.DataAnnotations;

namespace ProductManager.Application.DTOs.Identity;

public class RegisterUserDto
{
    [Required] 
    public string Username { get; set; } = null!;

    [EmailAddress] 
    [Required] 
    public string Email { get; set; } = null!;

    [Required]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = null!;
}