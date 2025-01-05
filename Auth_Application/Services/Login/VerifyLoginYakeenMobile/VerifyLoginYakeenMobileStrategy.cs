using Auth_Application.Models.LoginModels;
using Auth_Application.Validations;
using Auth_Core;
using Microsoft.AspNetCore.Identity;

namespace Auth_Application.Services.Login
{
	public class VerifyLoginYakeenMobileStrategy : VerifyLoginYakeenMobileService
	{
		private UserManager<ApplicationUser<string>> _userManager { get; }

		public VerifyLoginYakeenMobileStrategy(UserManager<ApplicationUser<string>> userManager)
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
