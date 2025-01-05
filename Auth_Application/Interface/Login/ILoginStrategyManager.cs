using Auth_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Interface.Login
{
	public interface ILoginStrategyManager
	{
		ILoginStrategy GetStrategy(LoginMethod loginMethod,LoginType loginType);
	}
}
