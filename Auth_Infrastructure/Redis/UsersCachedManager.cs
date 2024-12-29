using Auth_Core;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Infrastructure.Redis
{
	public class UsersCachedManager : IUsersCachedManager
	{
		private readonly IRedisCaching _redisCaching;
		private readonly IApplicationUserManager _applicationUserManager;

		public UsersCachedManager(IRedisCaching redisCaching, IApplicationUserManager applicationUserManager)
		{
			_redisCaching = redisCaching;
			_applicationUserManager = applicationUserManager;
		}

		public async Task<ApplicationUser<string>> GetUser(string emailOrNameKey)
		{
			if (string.IsNullOrEmpty(emailOrNameKey))
				return null;

			ApplicationUser<string> user = null;
			user = await _redisCaching.GetUserAsync(emailOrNameKey);
			if (user is null)
			{
				if(emailOrNameKey.Contains("@"))
					user = await _applicationUserManager.GetUserByEmailAsync(emailOrNameKey.Trim());
				//else nin

				if (user is null || string.IsNullOrEmpty(user.Email))
					return null;

				_redisCaching.SetUser(user.Email, "nin", user);
			}
			return user;
		}
	}
}
