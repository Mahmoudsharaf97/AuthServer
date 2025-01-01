using Auth_Application.Interface;
using Auth_Application.Interface.Login;
using Auth_Application.Models;
using Auth_Application.Models.LoginModels;
using Auth_Application.Validations;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase.Redis;
using Microsoft.AspNetCore.Identity;
using SME_Core;
using System.Security.AccessControl;

namespace Auth_Application.Services.VerifyLoginOTP
{
	public abstract class BaseVerifyLoginOTPServices : ILoginStrategy
	{
		private readonly IUsersCachedManager _usersCachedManager;
		private readonly IOtpService _otpService;
		private readonly IRedisCaching _cacheManager;
		private readonly ITokenServices _tokenServices;
		public UserManager<ApplicationUser<string>> _userManager { get; }
		public SignInManager<ApplicationUser<string>> _signInManager { get; }
		public Utilities _utilities { get; }
		protected BaseVerifyLoginOTPServices(IUsersCachedManager usersCachedManager, IOtpService otpService, SignInManager<ApplicationUser<string>> signInManager, Utilities utilities, IRedisCaching cacheManager, ITokenServices tokenServices, UserManager<ApplicationUser<string>> userManager)
		{
			_usersCachedManager = usersCachedManager;
			_otpService = otpService;
			_signInManager = signInManager;
			_utilities = utilities;
			_cacheManager = cacheManager;
			_tokenServices = tokenServices;
			_userManager = userManager;
		}

		protected BaseVerifyLoginOTPServices()
		{
			
		}

		protected abstract void ValidateUser(ApplicationUser<string> user);
		public async void VerifyLoginOtp(VerifyLoginOTPModel model)
		{
			// count num of tries and check if blocked 

			VerifyLoginOTPOutPut output = new();
			if (model == null)
				throw new AppException(ExceptionEnum.ModelIsEmpty);
			model.ValidateModel();

			ApplicationUser<string> user = await _usersCachedManager.GetUserByEmailOrNationalIdAsync(model.LoginType, LoginType.NationalId == model.LoginType ? model.NationalId : model.Email);
			ValidateUser(user);
			// Add and validate user DOB 
			OtpInfo otpInfo = await _otpService.GetCachedOtpInfoAsync(user.PhoneNumber);
			ValidateOtpInfo(otpInfo, model);

			

			await UserSignIn(user, user.Email, user.PasswordHash); 
			await _otpService.DeleteCachedOtpInfoAsync(user.PhoneNumber);
			//return output success 
		}
		protected async Task<LogInOutput> UserSignIn(ApplicationUser<string> user ,string email, string password)
		{
			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
			if (!isPasswordCorrect)
				return null;
			var result = await _signInManager.PasswordSignInAsync(email, password, false, true); ;
			if (!result.Succeeded)
				return null;

			user.LastSuccessLogin = DateTime.Now;
			//  await _userManager.UpdateAsync(); need to add to rabbiteMQ for Update 
			string sessionId = (await SetUserSessionAsync(user, email))?.SessionId;
			LogInOutput output = (await GetAccessToken(user,sessionId))?.Result;

			return output;
		}
		protected async Task<SessionStatus> SetUserSessionAsync(ApplicationUser<string> user ,string email)
		{
			SessionStatus userSession = new(Guid.NewGuid().ToString(), user.Id, _utilities.GetUserIP(), _utilities.GetUserAgent(), _utilities.GetMacAddress(), _utilities.GetUserIP());
			await _cacheManager.SetSessionAsync(email, userSession);
			return userSession;
		}
		protected async Task<IdentityOutput> GetAccessToken(ApplicationUser<string> user, string sessionId)
		{
			IdentityOutput tokenResult = await _tokenServices.GetAccessToken(user, sessionId);
			return tokenResult;
		}
		protected void ValidateOtpInfo(OtpInfo otpInfo, VerifyLoginOTPModel model)
		{
			if (otpInfo == null)
				throw new AppException(ExceptionEnum.ErrorOTPCompare);
			else if (otpInfo.VerificationCode != model.OTP)
				throw new AppException(ExceptionEnum.ErrorOTPCompare);
			else if (otpInfo.CreatedDate.AddMinutes(10) < DateTime.Now)
				throw new AppException(ExceptionEnum.ErrorOTPExpire);
		}
	}
}
