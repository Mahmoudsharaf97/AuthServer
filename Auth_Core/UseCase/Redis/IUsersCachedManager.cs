using Auth_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Core.UseCase.Redis
{
	public interface IUsersCachedManager
	{
		Task<ApplicationUser<string>> GetUserAsync(LoginType LoginType, string emailOrNinKey);
	}
}
