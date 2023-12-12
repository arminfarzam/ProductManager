using MediatR;
using ProductManager.Application.DTOs.Identity;
using ProductManager.Application.Services.Interfaces;

namespace ProductManager.Application.Features.Authentication.Command;

public class RegisterUserCommand:IRequest
{
    public RegisterUserDto RegisterDto { get; }
    public RegisterUserCommand(RegisterUserDto registerDto)
    {
        RegisterDto = registerDto;
    }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IAuthenticationService _authenticationService;
    public RegisterUserCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authenticationService.RegisterUser(request.RegisterDto);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}