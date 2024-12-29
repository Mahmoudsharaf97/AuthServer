using Auth_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Validations
{
	public static class ValidateUser
	{
		public static ApplicationUser<string> IsValidUser( this ApplicationUser<string> user)
		{
			if (user == null)
				throw new AppException(ExceptionEnum.UserNotFound);

			if (user.IsDeleted)
				throw new AppException(ExceptionEnum.UserDeleted);

			if (!user.PhoneNumberConfirmed)
				throw new AppException(ExceptionEnum.UserPhoneNotActiveConfirmed);

			if (!user.EmailConfirmed)
				throw new AppException(ExceptionEnum.UserEmailNotconfirmed);

			if (user.LockoutEnd != null)
				throw new AppException(ExceptionEnum.UserIsLocked);

			return user;

		}
	}
}
