using Auth_Application.Interface;
using Auth_Application.Models;
using Auth_Core;
using Auth_Core.UseCase.Redis;
using Microsoft.AspNetCore.Identity;
using SME_Core;
namespace Auth_Application.Services.Token
{
    public  class UserSignInService :IUserSignInService
    {
        private readonly UserManager<ApplicationUser<string>> _userManager;
        private readonly ITokenServices _tokenServices;
        private readonly IRedisCaching _redisCaching;

        public SignInManager<ApplicationUser<string>> _signInManager { get; }
        public Utilities _utilities { get; }

        public UserSignInService(UserManager<ApplicationUser<string>> userManager,
            SignInManager<ApplicationUser<string>> signInManager,
            ITokenServices tokenServices,Utilities utilities, IRedisCaching redisCaching)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
            _utilities = utilities;
            _redisCaching = redisCaching;
        }

        public  async Task<LogInOutput> UserSignIn(ApplicationUser<string> user, string email, string password)
        {
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordCorrect)
                throw new AppException(ExceptionEnum.login_incorrect_password_message);
            var result = await _signInManager.PasswordSignInAsync(email, password, false, true); ;
            if (!result.Succeeded)
                throw new AppException(ExceptionEnum.login_incorrect_password_message);

            user.LastSuccessLogin = DateTime.Now;
            //  await _userManager.UpdateAsync(); need to add to rabbiteMQ for Update 
            string sessionId = (await SetUserSessionAsync(user, email))?.SessionId;
            LogInOutput output = (await GetAccessToken(user, sessionId))?.Result;
            return output;
        }

        private async Task<IdentityOutput> GetAccessToken(ApplicationUser<string> user, string sessionId)
        {
            IdentityOutput tokenResult = await _tokenServices.GetAccessToken(user, sessionId);
            return tokenResult;
        }

        private async Task<SessionStatus> SetUserSessionAsync(ApplicationUser<string> user, string email)
        {
            SessionStatus userSession = new(Guid.NewGuid().ToString(), user.Id,
                _utilities.GetUserIP(), _utilities.GetUserAgent(), _utilities.GetMacAddress(), 
                _utilities.GetUserIP());
            await _redisCaching.SetSessionAsync(email, userSession);
            return userSession;
        }
    }
}
