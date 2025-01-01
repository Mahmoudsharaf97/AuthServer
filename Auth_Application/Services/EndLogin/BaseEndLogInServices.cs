using IdentityApplication.Interface;
using IdentityApplication.Models;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auth_Core.Global;
using Auth_Application.Models;
using Auth_Core;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using SME_Core;
using Auth_Core.Enums;
using Auth_Application.Services;

namespace Auth_Application.Services
{
    public  class BaseEndLogInServices
    {
        public SignInManager<ApplicationUser<string>> _signInManager { get; }
        public IRedisCaching _cacheManager { get; }
        public Utilities _utilities { get; }
        public AppSettingsConfiguration  _setting { get; }
        public BaseEndLogInServices(SignInManager<ApplicationUser<string>> signInManager
            , IRedisCaching cacheManager, AppSettingsConfiguration setting,Utilities utilities)
        {
            this._signInManager = signInManager;
            this._cacheManager = cacheManager;  
            this._setting = setting;
            this._utilities = utilities;
        }
        protected void ValidateUser (ApplicationUser<string>? user)
        {
            if (user == null)
                throw new AppException(ExceptionEnum.UserNotFound);

            if (user.IsDeleted)
                throw new AppException(ExceptionEnum.UserDeleted);

            if (!user.PhoneNumberConfirmed)
                throw new AppException(ExceptionEnum.UserPhoneNotActiveConfirmed);

            if (!user.EmailConfirmed)
                throw new AppException(ExceptionEnum.UserEmailNotconfirmed);

            if (user.LockoutEnd != null)
                throw new AppException(ExceptionEnum.UserIsLocked);
        }
        protected async Task<SignInResult> SignInAsync(string email ,string password)
        {
           return  await _signInManager.PasswordSignInAsync(email, password, false, true);
        }
        protected async Task<bool> SetUserSessionAsync(string email, SessionStatus sessionStatus)
        {
            return await _cacheManager.SetSessionAsync(email, sessionStatus);
        }

        public async Task<IdentityOutput> GetAccessToken(ApplicationUser<string>? user , string SessionId)
        {
            //var user = await _userManager.FindByIdAsync(userId);
            //if (user == null)
               // throw new AppException(ExceptionEnum.RecordNotExist);
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
                Expires = DateTime.UtcNow.AddMinutes(_setting.JwtTokenExpiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setting.JwtSecretKey)), SecurityAlgorithms.HmacSha256Signature)
            };
            string refreshToken = _utilities.GenerateRefreshToken();
            await _cacheManager.SetRefreshTokenAsync(user.Email,refreshToken);
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            IdentityOutput output = new(new LogInOutput(true, token), ErrorCodes.Success, _setting.JwtTokenExpiryMinutes, user.Email, DateTime.Now.AddMinutes(_setting.JwtTokenExpiryMinutes), user.Email.Split("@")[0]);
            output.Result.AccessTokenExpiration = output.TokenExpiryDate;
            output.Result.RefreshToken = refreshToken;
            return output;
        }
    }
}
