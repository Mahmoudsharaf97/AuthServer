using Auth_Core;
namespace Auth_Application.Validations
{
	public static class ValidateUser
	{
		public static ApplicationUser<string> IsValidUser( this ApplicationUser<string> user)
		{
			user.IsFound().IsNotDeleted().IsPhoneNumberConfirmed().IsEmailConfirmed().IsLockout();
			return user;
		}
		public static ApplicationUser<string> IsValidUserByNationalId( this ApplicationUser<string> user)
		{
			user.IsFound().IsNotDeleted().IsPhoneNumberConfirmed().IsEmailConfirmed().IsLockout();
			return user;
		}
		public static ApplicationUser<string> IsFoundUserNotConfirmed(this ApplicationUser<string> user)
		{
			user.IsFound().IsNotDeleted().IsLockout();
			return user;
		}
		public static ApplicationUser<string> IsFoundUserEmailNotConfirmed( this ApplicationUser<string> user)
		{
			user.IsFound().IsNotDeleted().IsPhoneNumberConfirmed().IsLockout();
			return user;
		}
		public static ApplicationUser<string> IsFoundUserPhoneNumberNotConfirmed( this ApplicationUser<string> user)
		{
			user.IsFound().IsNotDeleted().IsEmailConfirmed().IsLockout();
			return user;
		}
		public static ApplicationUser<string> IsFound( this ApplicationUser<string> user)
		{
			if (user == null)
				throw new AppException(ExceptionEnum.UserNotFound);
			return user;
		}
		public static ApplicationUser<string> IsFoundUserByNationalId( this ApplicationUser<string> user)
		{
			if (user == null)
				throw new AppException(ExceptionEnum.NoAcouuntToId);
			user.IsNotDeleted().IsLockout();
			return user;
		}
		public static ApplicationUser<string> IsFoundUserByEmail( this ApplicationUser<string> user)
		{
			if (user == null)
				throw new AppException(ExceptionEnum.login_incorrect_email_message);
			user.IsEmailConfirmed().IsLockout();
			return user;
		}
		public static ApplicationUser<string> IsNotDeleted( this ApplicationUser<string> user)
		{
			if (user.IsDeleted)
				throw new AppException(ExceptionEnum.UserDeleted);
			return user;
		}
		public static ApplicationUser<string> IsPhoneNumberConfirmed( this ApplicationUser<string> user)
		{
			if (!user.PhoneNumberConfirmed)
				throw new AppException(ExceptionEnum.UserPhoneNotActiveConfirmed);
			return user;
		}
		public static ApplicationUser<string> IsEmailConfirmed( this ApplicationUser<string> user)
		{
			if (!user.EmailConfirmed)
				throw new AppException(ExceptionEnum.UserEmailNotconfirmed);
			return user;
		}
		public static ApplicationUser<string> IsLockout( this ApplicationUser<string> user)
		{
			if (user.LockoutEnd == null || user.LockoutEnd > DateTime.UtcNow || user.LockoutEnabled) // user.LockoutEndDateUtc > DateTime.UtcNow 
				throw new AppException(ExceptionEnum.UserIsLocked);
			return user;

		}
		public static bool IsPhoneVerifiedByYakeen(this ApplicationUser<string> user)
		{
			if (!user.IsPhoneVerifiedByYakeen || string.IsNullOrEmpty(user.PhoneNumber))
				return false;
			return true;
		}
		public static bool IsYakeenNationalIdVerified(this ApplicationUser<string> user)
		{
			//if (!user.IsYakeenNationalIdVerified || string.IsNullOrEmpty(user.NationalId.ToString()))
			//	return false;
			return true;
		}
	}
}
