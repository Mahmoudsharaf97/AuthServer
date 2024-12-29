using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

		public async Task<ApplicationUser<string>> GetUserAsync(LoginType LoginType, string emailOrNinKey)
		{
			if (string.IsNullOrEmpty(emailOrNinKey))
				return null;

			ApplicationUser<string> user = null;
			user = await _redisCaching.GetUserAsync(emailOrNinKey);
			if (user is null)
			{
				if (LoginType == LoginType.Email)
					user = await _applicationUserManager.GetUserByEmailAsync(emailOrNinKey.Trim());
				else if (LoginType == LoginType.NationalId)
					user = await _applicationUserManager.GetUserByNationalId(long.Parse(emailOrNinKey));
				else
					return null;

				if (user is null || string.IsNullOrEmpty(user.Email))
					return null;

				_redisCaching.SetUser(user.Email,user.NationalId.ToString(), user);
			}
			return user;
		}
	}
}
