using Auth_Application.Interface;
using Auth_Application.Models.LoginModels.LoginInput;
using Auth_Application.Validations;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase.Redis;
using Auth_Core.UseCase;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Services.Login.LoginNationalIdConfirmation
{
	public class LoginNationalIdConfirmationStrategy : BaseLoginAccountConfirmationService
	{
		public override string StrategyName => $"{LoginMethod.Login}-{LoginType.Email}";
		private UserManager<ApplicationUser<string>> _userManager { get; }

		public LoginNationalIdConfirmationStrategy(UserManager<ApplicationUser<string>> userManager, IUsersCachedManager usersCachedManager, IOtpService otpService, AppSettingsConfiguration appSettings, IApplicationUserManager applicationUserManager) : base(usersCachedManager,otpService,appSettings,applicationUserManager)
		{
			_userManager = userManager;
		}

		protected override async Task ValidateUser(ApplicationUser<string> user, LoginConfirmationModel model)
		{
			user.IsFoundUserByEmail();
			bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.PWD);
			if (!isPasswordCorrect)
				throw new AppException(ExceptionEnum.login_incorrect_password_message);
		}
	}
}
