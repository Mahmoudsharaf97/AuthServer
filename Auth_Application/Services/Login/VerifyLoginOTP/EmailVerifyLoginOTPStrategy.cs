using Auth_Application.Models.LoginModels;
using Auth_Application.Validations;
using Auth_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Services.VerifyLoginOTP
{
	internal class EmailVerifyLoginOTPStrategy : BaseVerifyLoginOTPServices
	{
		protected override async Task ValidateUser(ApplicationUser<string> user, VerifyLoginOTPModel model)
		{
			user.IsFoundUserByEmail();
			bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.PWD);
			if (!isPasswordCorrect)
				throw new AppException(ExceptionEnum.login_incorrect_password_message);
		}
	}
}
