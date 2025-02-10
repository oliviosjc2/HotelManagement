using HM.Application.Response;
using HM.Application.Response.Auth;
using MediatR;

namespace HM.Application.Commands.Auth
{
    public class SignInCommand : IRequest<ResponseViewModel<SignInCommandResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
