using MediatR;

namespace AuthService.Application.CQRS.Authentication.Commands.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<string>;
}
