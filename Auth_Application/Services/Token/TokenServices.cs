using Auth_Application.Interface;
using Auth_Application.Models;
using Auth_Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Auth_Application.Services.Token
{
    public  class TokenServices : ITokenServices
    {
        private readonly AppSettingsConfiguration settings;
        public UserManager<ApplicationUser<string>> _userManager { get; }

        public TokenServices(AppSettingsConfiguration appSettingsConfiguration, UserManager<ApplicationUser<string>> userManager)
        {
            this.settings = appSettingsConfiguration;
            _userManager = userManager;
        }
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
            var output = new IdentityOutput
            {
                Result = new LogInOutput()
            };

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
                Expires = DateTime.UtcNow.AddHours(settings.JwtTokenExpiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSecretKey)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            output.ErrorCode = IdentityOutput.ErrorCodes.Success;
            output.Result = new LogInOutput();
            output.Result.AccessToken = token;
            output.Result.Success = true;
            output.TokenExpiryMinutes = settings.JwtTokenExpiryMinutes;
            output.Email = user.Email;
            output.TokenExpiryDate = DateTime.Now.AddMinutes(settings.JwtTokenExpiryMinutes);
            output.UserName = user.Email.Split("@")[0];
            return output;
        }
    }
}
