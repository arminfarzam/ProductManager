namespace ProductManager.Application.DTOs.Identity;

public class AuthResponseDto
{
    public string? AccessToken { get; set; }
    public DateTime Expiration { get; set; }
    public string? UserId { get; set; }
}