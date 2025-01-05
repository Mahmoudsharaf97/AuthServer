using Auth_Application.Interface;
using Auth_Application.Interface.Login;
using Auth_Application.Models;
using Auth_Application.Models.LoginModels.LoginInput;
using Auth_Application.Models.LoginModels.LoginOutput;
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
		private readonly AppSettingsConfiguration _appSettings;
		public UserManager<ApplicationUser<string>> _userManager { get; }
		public SignInManager<ApplicationUser<string>> _signInManager { get; }
		public Utilities _utilities { get; }
		protected BaseVerifyLoginOTPServices(IUsersCachedManager usersCachedManager, IOtpService otpService, SignInManager<ApplicationUser<string>> signInManager, Utilities utilities, IRedisCaching cacheManager, ITokenServices tokenServices, UserManager<ApplicationUser<string>> userManager, AppSettingsConfiguration appSettings)
		{
			_usersCachedManager = usersCachedManager;
			_otpService = otpService;
			_signInManager = signInManager;
			_utilities = utilities;
			_cacheManager = cacheManager;
			_tokenServices = tokenServices;
			_userManager = userManager;
			_appSettings = appSettings;
		}

		protected BaseVerifyLoginOTPServices()
		{
			
		}

		protected abstract Task ValidateUser(ApplicationUser<string> user, VerifyLoginOTPModel model);
		public async Task<GenericOutput<VerifyLoginOTPOutput>> VerifyLoginOtp(VerifyLoginOTPModel model)
		{
			// count num of tries and check if blocked 
			if (model == null)
				throw new AppException(ExceptionEnum.ModelIsEmpty);
			model.ValidateModel();

			GenericOutput<VerifyLoginOTPOutput> output = new();
			output.Result = new();

			ApplicationUser<string> user = await _usersCachedManager.GetUserByEmailOrNationalIdAsync(model.LoginType, LoginType.NationalId == model.LoginType ? model.NationalId : model.UserName);
			await ValidateUser(user,model);
			// Add and validate user DOB 
			CheckUserConfirmedByYakeen(output, user);
			if (output.Result.LoginMethod == LoginMethod.VerifyLoginOTP)
			{
				OtpInfo otpInfo = await _otpService.GetCachedOtpInfoAsync(user.PhoneNumber);
				ValidateOtpInfo(otpInfo, model);
				await _otpService.DeleteCachedOtpInfoAsync(user.PhoneNumber);

				LogInOutput userSignInOutput = await UserSignIn(user, user.Email, user.PasswordHash);
				output.Result.AccessToken = userSignInOutput.AccessToken;
				output.Result.AccessTokenExpiration = userSignInOutput.AccessTokenExpiration.Value;
				output.Result.RefreshToken = userSignInOutput.RefreshToken;

				mapOutputModel(loginOutput: output.Result, user, otpInfo);
			}

			return output;
		}
		protected async Task<LogInOutput> UserSignIn(ApplicationUser<string> user ,string email, string password)
		{
			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
			if (!isPasswordCorrect)
				throw new AppException(ExceptionEnum.login_incorrect_password_message);
			var result = await _signInManager.PasswordSignInAsync(email, password, false, true); ;
			if (!result.Succeeded)
				throw new AppException(ExceptionEnum.login_incorrect_password_message);

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
		protected void CheckUserConfirmedByYakeen(GenericOutput<VerifyLoginOTPOutput> output, ApplicationUser<string> user)
		{
			if (!user.IsPhoneVerifiedByYakeen())
			{
				output.ErrorDetails = new(IsSuccess: false, ErrorCode: ExceptionEnum.login_incorrect_phonenumber_not_verifed, ErrorDescription: "login incorrect phonenumber not verifed");
				output.Result.LoginMethod = LoginMethod.VerifyYakeenMobile;
			}
			if (!user.IsYakeenNationalIdVerified())
			{
				output.ErrorDetails = new(IsSuccess: false, ErrorCode: ExceptionEnum.UserYakeenNationalIdNotVerified, ErrorDescription: "User Yakeen National Id Not Verified");
				output.Result.LoginMethod = LoginMethod.LoginAccountConfirmation;
			}
			output.Result.LoginMethod = LoginMethod.VerifyLoginOTP;
		}
		private void mapOutputModel(VerifyLoginOTPOutput loginOutput, ApplicationUser<string> user, OtpInfo otpInfo)
		{
			loginOutput.Email = user.Email;
			loginOutput.PhoneNumberConfirmed = true;
			loginOutput.PhoneNumber = user.PhoneNumber;
			loginOutput.PhoneNo = user.PhoneNumber;
			loginOutput.FullNameAr = user.FullNameAr;
			loginOutput.DisplayNameAr = user.FullNameAr;
			loginOutput.FullNameEn = user.FullNameEn;
			loginOutput.DisplayNameEn = user.FullNameEn;
			if (_appSettings.SendOtpInResponse)
				loginOutput.OTP = otpInfo.VerificationCode;
			loginOutput.PhoneVerification = true;
			loginOutput.IsYakeenChecked = true;
			loginOutput.NationalID = user.NationalId.ToString();
		}
	}
}
