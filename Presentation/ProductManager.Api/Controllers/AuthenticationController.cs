using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.DTOs.Identity;
using ProductManager.Application.Features.Authentication.Command;
using System.Net;

namespace ProductManager.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "RegisterUser")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerDto)
        {
            await _mediator.Send(new RegisterUserCommand(registerDto));
            return Ok();
        }

        [HttpPost(Name = "LoginUser")]
        [ProducesResponseType(typeof(AuthResponseDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginDto)=>
            Ok(await _mediator.Send(new LoginUserCommand(loginDto)));
        
    }
}
