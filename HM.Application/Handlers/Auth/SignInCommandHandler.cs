using HM.Application.Commands.Auth;
using HM.Application.Response;
using HM.Application.Response.Auth;
using HM.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace HM.Application.Handlers.Auth
{
    public class SignInCommandHandler
        : IRequestHandler<SignInCommand, ResponseViewModel<SignInCommandResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SignInCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseViewModel<SignInCommandResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                    return ResponseViewModel<SignInCommandResponse>.GetResponse(HttpStatusCode.BadRequest,
                        "Usuário e/ou senha incorretos.");

                if (!await _userManager.CheckPasswordAsync(user, request.Password))
                    return ResponseViewModel<SignInCommandResponse>.GetResponse(HttpStatusCode.BadRequest,
                        "Usuário e/ou senha incorretos.");

                string accessToken = await GenerateAccessToken(user);
                string refreshToken = GenerateRefreshToken();

                await StoreRefreshToken(user, refreshToken);

                var response = new SignInCommandResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

                return ResponseViewModel<SignInCommandResponse>.GetResponse(HttpStatusCode.OK, "Login realizado com sucesso!",
                    response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            { 
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("2b922a7b1d3521efd88a700abdeadbfa90b17c1f43a340cc168bb8bea759e33f"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "HM.API",
                "HM.CLIENT",
                claims,
                expires: DateTime.Now.AddMinutes(45),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var refreshToken = Guid.NewGuid().ToString();
            return refreshToken;
        }

        private async Task StoreRefreshToken(ApplicationUser user, string refreshToken)
        {
            user.RefreshTokenExpiresIn = DateTime.UtcNow.AddDays(7);
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);
        }
    }
}
