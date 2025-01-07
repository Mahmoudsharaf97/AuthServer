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

		public ILoginStrategy GetStrategy(LoginMethod loginMethod, LoginType loginType)
		{
			return _strategies?.FirstOrDefault(s => s.StrategyName == $"{loginMethod}-{loginType}");
		}
	}
}
