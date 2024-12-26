using Auth_Application.Models;
using Auth_Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace IdentityApplication.Services
{
    public partial class IdentityServices
    {
        public string GenerateTokenJWT(string ID, string Email, string userName, string sessionId)
        {
            var claims = new[]

                       {
                          new Claim(ClaimTypes.NameIdentifier, ID),

                          new Claim(ClaimTypes.Email, Email),

                          new Claim(ClaimTypes.Name, sessionId),

                          new Claim(ClaimTypes.PrimarySid, "123"),

                          new Claim(ClaimTypes.SerialNumber, sessionId),

                          new Claim("auth_time", DateTime.Now.ToString()),
                        };
            var secrectkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSecretKey));

            var creds = new SigningCredentials(secrectkey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken("Bcare",
              "Bcare",
              claims,
              expires: DateTime.Now.AddMinutes(settings.JwtTokenExpiryMinutes),
              signingCredentials: creds);
            //  key = x.ToString();
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedJwt;

        }
        public async Task<IdentityOutput> GetToken(string userId, string SessionId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new AppException(ExceptionEnum.RecordNotExist);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]
                          {
                        new Claim("Any","No"),
                        new Claim("UserId",user.Id.ToString()),
                        new Claim("SessionId",SessionId),
                        new Claim("Email",user.Email.ToString()),
                        new Claim("LockoutEnabled",user.LockoutEnabled.ToString()),
                        new Claim("LockoutEnd",user.LockoutEnd.ToString()),
                        new Claim("UserName",user.Email.Split("@")[0].ToString()),
                          }),
                Expires = DateTime.UtcNow.AddMinutes(settings.JwtTokenExpiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSecretKey)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
			IdentityOutput output = new(new LogInOutput(true, token), IdentityOutput.ErrorCodes.Success, settings.JwtTokenExpiryMinutes, user.Email, DateTime.Now.AddMinutes(settings.JwtTokenExpiryMinutes), user.Email.Split("@")[0]);
            return output;
        }

        public async Task SaveRefreshToken(string username, string refreshToken)
        {
            await _cacheManager.SetRefreshTokenAsync(username, refreshToken);
        }

        public async Task<bool> ValidateRefreshToken(string UserId, string refreshToken)
        {
            // Find the user by refresh token
            var rv = await _cacheManager.GetRefreshTokenAsync(UserId);
            if (rv != null && !string.IsNullOrEmpty(rv))
            {
                return string.Equals(rv, refreshToken, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
