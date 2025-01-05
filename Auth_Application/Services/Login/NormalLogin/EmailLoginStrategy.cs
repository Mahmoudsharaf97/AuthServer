using Auth_Application.Models.LoginModels;
using Auth_Application.Validations;
using Auth_Core;
using Microsoft.AspNetCore.Identity;
namespace Auth_Application.Services.Login.NormalLogin
{
	public class EmailLoginStrategy : BaseNormalLoginService
	{
		public UserManager<ApplicationUser<string>> _userManager { get; }

		public EmailLoginStrategy(UserManager<ApplicationUser<string>> userManager)
		{
			_userManager = userManager;
		}

		protected override async Task ValidateUser(ApplicationUser<string> user, LoginModel model)
		{
			user.IsFoundUserByEmail();
			bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.PWD);
			if (!isPasswordCorrect)
				throw new AppException(ExceptionEnum.login_incorrect_password_message);
		}
	}
}
