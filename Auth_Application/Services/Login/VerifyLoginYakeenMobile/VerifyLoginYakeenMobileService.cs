using Auth_Application.Interface;
using Auth_Application.Interface.Login;
using Auth_Application.Models.LoginModels;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using Microsoft.AspNetCore.Identity;

namespace Auth_Application.Services.Login
{
	public abstract class VerifyLoginYakeenMobileService : ILoginStrategy
	{
		private readonly IUsersCachedManager _usersCachedManager;
		private readonly IApplicationUserManager _applicationUserManager;
		private readonly IOtpService _otpService;
		public UserManager<ApplicationUser<string>> _userManager { get; }

		protected VerifyLoginYakeenMobileService(IUsersCachedManager usersCachedManager, UserManager<ApplicationUser<string>> userManager, IApplicationUserManager applicationUserManager, IOtpService otpService)
		{
			_usersCachedManager = usersCachedManager;
			_userManager = userManager;
			_applicationUserManager = applicationUserManager;
			_otpService = otpService;
		}

		protected abstract void ValidateUser(ApplicationUser<string> user);

		public async void VerifyLoginYakeenMobile(VerifyLoginYakeenMobile model)// return base generic output
		{
			if (model is null)
				throw new AppException(ExceptionEnum.ModelIsEmpty);
			model.ValidateModel();

			// 1- check if user is locked 
			ApplicationUser<string> user = await _usersCachedManager.GetUserByEmailOrNationalIdAsync(model.LoginType, model.UserName);
			ValidateUser(user); // null, email confirmd, userLocked,
			bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.PWD);
			if (!isPasswordCorrect)
				return; // throw proper message

			bool IsEmailBelongsToOtherUser = await _applicationUserManager.CheckNationalIdBelongsForDifferentEmail(long.Parse( model.NationalId),user.Email);
			if (true)
				return; // throw error 

			// 2- VerifyMobileFromYakeen

			// 3- create otpModel and send it 
			int otp = _otpService.GenerateRandomOTP();
		}
	}
}
