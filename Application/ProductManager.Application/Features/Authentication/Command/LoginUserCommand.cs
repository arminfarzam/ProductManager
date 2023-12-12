using MediatR;
using ProductManager.Application.DTOs.Identity;
using ProductManager.Application.Services.Interfaces;

namespace ProductManager.Application.Features.Authentication.Command;

public class LoginUserCommand:IRequest<AuthResponseDto>
{
    public LoginUserDto LoginDto { get; set; }
    public LoginUserCommand(LoginUserDto loginDto)
    {
        LoginDto = loginDto;
    }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponseDto>
{
    private readonly IAuthenticationService _authenticationService;
    public LoginUserCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _authenticationService.LoginUser(request.LoginDto);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}