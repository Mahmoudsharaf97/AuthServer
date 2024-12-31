using Auth_Application.Interface.Login;
using Auth_Application.Models;
using Auth_Core.Global;
using Auth_Core.UseCase.Redis;
using Auth_Core.UseCase;
using Auth_Core;
using IdentityApplication.Interface;
using Microsoft.AspNetCore.Identity;
using SME_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth_Core.Enums;

namespace Auth_Application.Services.Login
{
    public class NationalIdBeginLoginStrategy : BaseEndLogInServices, ILoginStrategy
    {
        //private readonly IApplicationUserManager _applicationUserManager;
        private readonly IUsersCachedManager _usersCachedManager;
        private readonly GlobalInfo globalInfo;
        public UserManager<ApplicationUser<string>> _userManager { get; }
        public ISessionServices _sessionServices { get; }
        public Utilities _utilities { get; }
        public AppSettingsConfiguration _settings;
        //public IRedisCaching _cacheManager { get; }
		public NationalIdBeginLoginStrategy(SignInManager<ApplicationUser<string>> signInManager
			, IRedisCaching cacheManager, UserManager<ApplicationUser<string>> userManager
			, AppSettingsConfiguration settings, Utilities utilities, IUsersCachedManager usersCachedManager)
			: base(signInManager, cacheManager, settings,utilities)
		{
			//this._cacheManager = cacheManager;
			this._userManager = userManager;
			this._settings = settings;
			this._utilities = utilities;
			_usersCachedManager = usersCachedManager;
		}
		public async Task<LogInOutput> LoginByNAtionalId(long nationalId, string password)
        {
            try
            {
                var user = await _usersCachedManager.GetUserByEmailOrNationalIdAsync(LoginType.NationalId, nationalId.ToString());

				ValidateUser(user);
                var result = await SignInAsync(user.Email!, password);
                if (result.Succeeded)
                {
                    var userSession = new SessionStatus(Guid.NewGuid().ToString(), user.Id, _utilities.GetUserIP(), _utilities.GetUserAgent(), _utilities.GetMacAddress(), _utilities.GetUserIP());
                    await SetUserSessionAsync(user.Email!, userSession);
                    user.LastSuccessLogin = DateTime.Now;
                    //  await _userManager.UpdateAsync(); need to add to rabbiteMQ for Update 
                    var tokenResult = await GetAccessToken(user, userSession.SessionId);
                    if (tokenResult.ErrorCode == IdentityOutput.ErrorCodes.Success)
                    {
                        LogInOutput output = tokenResult.Result;
                        return output;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                var x = ex;
                throw new AppException(ExceptionEnum.UserLoginDataNotCorrect);
            }
        }
    }
}

