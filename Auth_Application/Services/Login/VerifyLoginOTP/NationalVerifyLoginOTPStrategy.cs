using Auth_Application.Models.LoginModels;
using Auth_Application.Validations;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Services.VerifyLoginOTP
{
	public class NationalVerifyLoginOTPStrategy : BaseVerifyLoginOTPServices
	{
		protected override void ValidateUser(ApplicationUser<string> user)
		{
			user.IsFoundUserByNationalId();
		}
	}
}
