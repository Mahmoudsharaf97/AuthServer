using Auth_Application.Interface.Login;
using Auth_Application.Services.Login.LoginNationalIdConfirmation;
using Auth_Application.Services.Login.NormalLogin;
using Auth_Application.Services.VerifyLoginOTP;
using Auth_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Services.Login
{
	public class LoginStrategyManager : ILoginStrategyManager
	{
		private readonly IEnumerable<ILoginStrategy> _strategies;

		public LoginStrategyManager(IEnumerable<ILoginStrategy> strategies)
		{
			_strategies = strategies;
		}

		public ILoginStrategy SetStrategy(LoginMethod loginMethod)
		{
			return loginMethod switch
			{
				LoginMethod.Login => _strategies.OfType<BaseNormalLoginService>().FirstOrDefault(),
				LoginMethod.VerifyYakeenMobile => _strategies.OfType<VerifyLoginYakeenMobileService>().FirstOrDefault(),
				LoginMethod.LoginAccountConfirmation => _strategies.OfType<BaseLoginAccountConfirmationService>().FirstOrDefault(),
				LoginMethod.VerifyLoginOTP => _strategies.OfType<BaseVerifyLoginOTPServices>().FirstOrDefault()
			};
		}
	}
}
