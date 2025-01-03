using Auth_Application.Validations;
using Auth_Core;

namespace Auth_Application.Services.Login
{
	public class VerifyLoginYakeenMobileStrategy : VerifyLoginYakeenMobileService
	{
		protected override void ValidateUser(ApplicationUser<string> user)
		{
			user.IsFoundUserByEmail();
		}
	}
}
