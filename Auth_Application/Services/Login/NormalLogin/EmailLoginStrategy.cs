using Auth_Application.Interface;
using Auth_Application.Models.LoginModels.LoginInput;
using Auth_Application.Validations;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase.Redis;
using Microsoft.AspNetCore.Identity;
namespace Auth_Application.Services.Login.NormalLogin
{
	public class EmailLoginStrategy : BaseNormalLoginService
	{
		public override string StrategyName => $"{LoginMethod.Login}-{LoginType.Email}";
		public UserManager<ApplicationUser<string>> _userManager { get; }

		public EmailLoginStrategy(UserManager<ApplicationUser<string>> userManager, IUsersCachedManager usersCachedManager, IOtpService otpService, AppSettingsConfiguration appSettings):base(usersCachedManager,otpService,appSettings)
		{
			_userManager = userManager;
		}

		protected override async Task ValidateUser(ApplicationUser<string> user, NormalLoginModel model)
		{
			user.IsFoundUserByEmail();
			bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.PWD);
			if (!isPasswordCorrect)
				throw new AppException(ExceptionEnum.login_incorrect_password_message);
		}
	}
}
