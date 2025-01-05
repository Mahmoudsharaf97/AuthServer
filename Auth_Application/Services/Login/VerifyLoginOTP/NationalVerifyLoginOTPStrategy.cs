using Auth_Application.Models.LoginModels.LoginInput;
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
		public override string StrategyName => $"{LoginMethod.VerifyLoginOTP}-{LoginType.NationalId}";

		protected override async Task ValidateUser(ApplicationUser<string> user, VerifyLoginOTPModel model)
		{
			user.IsFoundUserByNationalId();
		}
	}
}
