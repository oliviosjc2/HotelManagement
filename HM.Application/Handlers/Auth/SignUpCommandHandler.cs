using HM.Application.Commands.Auth;
using HM.Application.Response;
using HM.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace HM.Application.Handlers.Auth
{
    public class SignUpCommandHandler
        : IRequestHandler<SignUpCommand, ResponseViewModel<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SignUpCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseViewModel<string>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user is not null)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest,
                        "O e-mail informado já possui cadastro ativo no sistema.");

                user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, "Falha na criação do usuário.");

                var addToRoleResult = await _userManager.AddToRoleAsync(user, "FINAL_CUSTOMER");
                if (!addToRoleResult.Succeeded)
                    return ResponseViewModel<string>.GetResponse(HttpStatusCode.BadRequest, "Falha ao atribuir a role FINAL_CUSTOMER.");

                return ResponseViewModel<string>.GetResponse(HttpStatusCode.OK, "Usuário criado e atribuído à role FINAL_CUSTOMER com sucesso.");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
