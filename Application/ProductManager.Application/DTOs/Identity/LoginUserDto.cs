using System.ComponentModel.DataAnnotations;

namespace ProductManager.Application.DTOs.Identity;

public class LoginUserDto
{
    [Required] 
    public string Username { get; set; } = null!;


    [Required] 
    public string Password { get; set; } = null!;
}