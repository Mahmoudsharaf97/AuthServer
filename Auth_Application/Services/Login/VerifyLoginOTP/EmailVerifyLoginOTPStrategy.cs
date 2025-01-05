using Auth_Application.Interface;
using Auth_Application.Models.LoginModels.LoginInput;
using Auth_Application.Validations;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase.Redis;
using Microsoft.AspNetCore.Identity;
using SME_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Services.VerifyLoginOTP
{
	internal class EmailVerifyLoginOTPStrategy : BaseVerifyLoginOTPServices
	{
		public EmailVerifyLoginOTPStrategy(IUsersCachedManager usersCachedManager, IOtpService otpService, SignInManager<ApplicationUser<string>> signInManager, Utilities utilities, IRedisCaching cacheManager, ITokenServices tokenServices
			, UserManager<ApplicationUser<string>> userManager, AppSettingsConfiguration appSettings)
			:base(usersCachedManager,otpService,signInManager,utilities,cacheManager,tokenServices,userManager,appSettings)
		{
			
		}
		public override string StrategyName => $"{LoginMethod.VerifyLoginOTP}-{LoginType.Email}";

		protected override async Task ValidateUser(ApplicationUser<string> user, VerifyLoginOTPModel model)
		{
			user.IsFoundUserByEmail();
			bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.PWD);
			if (!isPasswordCorrect)
				throw new AppException(ExceptionEnum.login_incorrect_password_message);
		}
	}
}
