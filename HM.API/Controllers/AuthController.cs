using HM.API.Utils;
using HM.Application.Commands.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return await ResponsePatternUtil.BaseResponse(result);
            }
            catch(Exception)
            {
                throw;
            }
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command,
                    CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _mediator.Send(command, cancellationToken);
                return await ResponsePatternUtil.BaseResponse(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
