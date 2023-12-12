using ProductManager.Application.DTOs.Identity;

namespace ProductManager.Application.Services.Interfaces;

public interface IAuthenticationService
{
    Task RegisterUser(RegisterUserDto registerUserDto);
    Task<AuthResponseDto> LoginUser(LoginUserDto loginUserDto);
}