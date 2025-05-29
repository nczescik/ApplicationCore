using AuthService.Application.Authentication.Commands.Login;
using AuthService.Application.DTOs;
using AuthService.Application.User.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(
            IMediator mediator
            )
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Invalid login request.");
            }

            var token = await _mediator.Send(new LoginCommand(request.Username, request.Password), cancellationToken);

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var userId = await _mediator.Send(new CreateUserCommand(request.Username, request.Email, request.Password), cancellationToken);

            return Ok(userId);
        }
    }
}
