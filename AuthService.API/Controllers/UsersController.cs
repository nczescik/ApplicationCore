using AuthService.Application.User.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(
            IMediator mediator
            )
        {
            _mediator = mediator;
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var userDto = await _mediator.Send(new GetUserQuery { UserId = id });
            if (userDto == null)
                return NotFound();

            return Ok(userDto);
        }
    }
}
