using MediatR;

namespace AuthService.Application.Authentication.Commands.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<string>;
}
