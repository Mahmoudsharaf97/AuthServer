using Auth_Core;
using IdentityApplication.Features;
using IdentityApplication.Models;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth_Application.Features
{
    public class GetAccessTokenHandler : IRequestHandler<GetAccessTokenQuery, TokenResponse>
    {
        public AppSettingsConfiguration _settings { get; }

        public GetAccessTokenHandler(AppSettingsConfiguration settings)
        {
                this._settings = settings;  
        }
        public async Task<TokenResponse> Handle(GetAccessTokenQuery request, CancellationToken cancellationToken)
        {

            TokenResponse output = new();
                var tokenDescriptor = new SecurityTokenDescriptor
                {

                    Subject = new ClaimsIdentity(new Claim[]
                              {
                        new Claim("Any","Yes"),
                        new Claim("SessionId",Guid.NewGuid().ToString()),
                        //new Claim("Email",null),
                        new Claim("LockoutEnabled",DateTime.Now.ToString()),
                        //new Claim("LockoutEnd",user.LockoutEnd.ToString()),
                        //new Claim("UserName",user.Email.Split("@")[0].ToString()),
                              }),
                    Expires = DateTime.UtcNow.AddHours(_settings.JwtTokenExpiryMinutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtSecretKey)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                output.Token = token;
                output.CanPurchase = true;
                output.Expires_in = _settings.JwtTokenExpiryMinutes;
                output.TokenExpirationDate = DateTime.Now.AddMinutes(_settings.JwtTokenExpiryMinutes);
                return output;
            }
        
    }
}
