using Auth_Application.Validations;
using Auth_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Services.Login.LoginNationalIdConfirmation
{
	public class LoginNationalIdConfirmationStrategy : BaseLoginAccountConfirmationService
	{
		protected override void ValidateUser(ApplicationUser<string> user)
		{
			user.IsFoundUserByEmail();
		}
	}
}
