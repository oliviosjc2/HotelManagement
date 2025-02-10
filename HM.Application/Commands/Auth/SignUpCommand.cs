using HM.Application.Response;
using MediatR;

namespace HM.Application.Commands.Auth
{
    public class SignUpCommand : IRequest<ResponseViewModel<string>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
