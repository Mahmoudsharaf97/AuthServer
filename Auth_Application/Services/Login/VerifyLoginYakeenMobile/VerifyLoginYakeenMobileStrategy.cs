using Auth_Application.Interface;
using Auth_Application.Models.LoginModels.LoginInput;
using Auth_Application.Validations;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase.Redis;
using Auth_Core.UseCase;
using Microsoft.AspNetCore.Identity;

namespace Auth_Application.Services.Login
{
	public class VerifyLoginYakeenMobileStrategy : VerifyLoginYakeenMobileService
	{
		public override string StrategyName => $"{LoginMethod.VerifyYakeenMobile}-{LoginType.Email}";

		private UserManager<ApplicationUser<string>> _userManager { get; }

		public VerifyLoginYakeenMobileStrategy(UserManager<ApplicationUser<string>> userManager, IUsersCachedManager usersCachedManager, IApplicationUserManager applicationUserManager, IOtpService otpService, AppSettingsConfiguration appSettings, IYakeenClient yakeenClient) 
			:base(usersCachedManager,applicationUserManager,otpService,appSettings, yakeenClient)
		{
			_userManager = userManager;
		}

		protected override async Task ValidateUser(ApplicationUser<string> user, VerifyLoginYakeenMobile model)
		{
			user.IsFoundUserByEmail();
			bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.PWD);
			if (!isPasswordCorrect)
				throw new AppException(ExceptionEnum.login_incorrect_password_message);
		}
	}
}
