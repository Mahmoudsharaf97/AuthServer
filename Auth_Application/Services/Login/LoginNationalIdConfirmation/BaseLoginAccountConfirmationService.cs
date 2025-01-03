using Auth_Application.Interface;
using Auth_Application.Interface.Login;
using Auth_Application.Models.LoginModels;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase.Redis;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Services.Login.LoginNationalIdConfirmation
{
	public abstract class BaseLoginAccountConfirmationService : ILoginStrategy
	{
		private readonly IUsersCachedManager _usersCachedManager;
		private readonly IOtpService _otpService;
		private UserManager<ApplicationUser<string>> _userManager { get; }

		protected BaseLoginAccountConfirmationService()
		{
			
		}

		protected BaseLoginAccountConfirmationService(IUsersCachedManager usersCachedManager, UserManager<ApplicationUser<string>> userManager, IOtpService otpService)
		{
			_usersCachedManager = usersCachedManager;
			_userManager = userManager;
			_otpService = otpService;
		}

		protected abstract void ValidateUser(ApplicationUser<string> user);
		public async void AccountConfirmation(LoginNationalIdConfirmationModel model)
		{
			// check if user num of tries locked and count new 

			if (model is null)
				throw new AppException(ExceptionEnum.ModelIsEmpty);
			model.ValidateModel();

			ApplicationUser<string> user = await _usersCachedManager.GetUserByEmailOrNationalIdAsync(model.LoginType, model.UserName);
			ValidateUser(user); // null, email confirmd, userLocked,
			bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.PWD);
			if (!isPasswordCorrect)
				return; // throw proper message

			// get user info form yakeen
			// create otpmodel and send otp 
			int otp = _otpService.GenerateRandomOTP();
		}
	}
}
