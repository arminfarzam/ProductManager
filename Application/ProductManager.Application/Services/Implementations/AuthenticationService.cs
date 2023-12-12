using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductManager.Application.DTOs.Configuration;
using ProductManager.Application.DTOs.Identity;
using ProductManager.Application.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductManager.Application.Services.Implementations;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtConfigurationDto _jwtConfiguration;
    public AuthenticationService(UserManager<IdentityUser> userManager, IOptions<JwtConfigurationDto> jwtOptions)
    {
        _userManager = userManager;
        _jwtConfiguration = jwtOptions.Value;
    }

    public async Task RegisterUser(RegisterUserDto registerUserDto)
    {
        var userExists = await _userManager.FindByNameAsync(registerUserDto.Username);
        if (userExists != null) throw new Exception("UserName is Already Exist");
        var user = new IdentityUser()
        {
            Email = registerUserDto.Email,
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            PhoneNumber = registerUserDto.PhoneNumber,
            PhoneNumberConfirmed = true,
            UserName = registerUserDto.Username,
        };
        var result = await _userManager.CreateAsync(user, registerUserDto.Password);
        if (!result.Succeeded)
            throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
    }

    public async Task<AuthResponseDto> LoginUser(LoginUserDto loginUserDto)
    {
        var user = await _userManager.FindByNameAsync(loginUserDto.Username);
        
        if (user != null && await _userManager.CheckPasswordAsync(user, loginUserDto.Password))
        {
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName!)
            };
            var token = CreateToken(authClaims);
            var result= new AuthResponseDto()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                UserId = user.Id
            };
            return result;
        }
        throw new Exception("UserName Or Password Is Invalid");
    }

    private JwtSecurityToken CreateToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret!));
        var token = new JwtSecurityToken(
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            expires: DateTime.Now.AddMinutes(_jwtConfiguration.TokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return token;
    }
}