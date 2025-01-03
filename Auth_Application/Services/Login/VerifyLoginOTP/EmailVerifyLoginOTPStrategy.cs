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
		protected override void ValidateUser(ApplicationUser<string> user)
		{
			user.IsFoundUserByEmail();
		}
	}
}
