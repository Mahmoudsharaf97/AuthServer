using Auth_Application.Interface;
using Auth_Application.Interface.Login;
using Auth_Application.Models;
using Auth_Application.Models.LoginModels.LoginInput;
using Auth_Application.Models.LoginModels.LoginOutput;
using Auth_Application.Validations;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Services.Login.LoginNationalIdConfirmation
{
	public abstract class BaseLoginAccountConfirmationService : LoginStrategy
	{
		private readonly IUsersCachedManager _usersCachedManager;
		private readonly IOtpService _otpService;
		private UserManager<ApplicationUser<string>> _userManager { get; }
		private readonly AppSettingsConfiguration _appSettings;
		private readonly IApplicationUserManager _applicationUserManager;

		protected BaseLoginAccountConfirmationService()
		{
			
		}
		public override Task<GenericOutput<BaseLoginOutput>> Execute(LoginInputModel loginInput)
		{
			throw new NotImplementedException();
		}
		protected BaseLoginAccountConfirmationService(IUsersCachedManager usersCachedManager, UserManager<ApplicationUser<string>> userManager, IOtpService otpService, AppSettingsConfiguration appSettings, IApplicationUserManager applicationUserManager)
		{
			_usersCachedManager = usersCachedManager;
			_userManager = userManager;
			_otpService = otpService;
			_appSettings = appSettings;
			_applicationUserManager = applicationUserManager;
		}

		protected abstract Task ValidateUser(ApplicationUser<string> user, LoginConfirmationModel model);
		public async Task<GenericOutput<LoginConfirmationOutput>> AccountConfirmation(LoginConfirmationModel model)
		{
			// check if user num of tries locked and count new 

			if (model is null)
				throw new AppException(ExceptionEnum.ModelIsEmpty);
			model.ValidateModel();

			GenericOutput<LoginConfirmationOutput> output = new();
			output.Result = new();

			ApplicationUser<string> user = await _usersCachedManager.GetUserByEmailOrNationalIdAsync(model.LoginType, model.UserName);
			await ValidateUser(user,model); // null, email confirmd, userLocked,

			bool IsEmailBelongsToOtherUser = await _applicationUserManager.CheckNationalIdBelongsForDifferentEmail(long.Parse(model.NationalId), user.Email);
			if (IsEmailBelongsToOtherUser)
				throw new AppException(ExceptionEnum.exist_nationalId_signup_error);
			
			// get user info form yakeen and edit user 

			CheckUserConfirmedByYakeen(output, user);
			if (output.Result.LoginMethod == LoginMethod.VerifyLoginOTP)
			{
				OtpInfo otpInfo = await _otpService.SendOtp(SMSType.LoginOtp, user);
				mapOutputModel(loginOutput: output.Result, user, otpInfo);
			}
			return output;
		}
		protected void CheckUserConfirmedByYakeen(GenericOutput<LoginConfirmationOutput> output, ApplicationUser<string> user)
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
		private void mapOutputModel(LoginConfirmationOutput loginOutput, ApplicationUser<string> user, OtpInfo otpInfo)
		{
			loginOutput.PhoneNumberConfirmed = true;
			loginOutput.PhoneNumber = user.PhoneNumber;
			loginOutput.PhoneNo = user.PhoneNumber;
			loginOutput.FullNameAr = user.FullNameAr;
			loginOutput.FullNameEn = user.FullNameEn;
			if (_appSettings.SendOtpInResponse)
				loginOutput.OTP = otpInfo.VerificationCode;
			loginOutput.PhoneVerification = true;
			loginOutput.IsYakeenChecked = true;
			loginOutput.NationalID = user.NationalId.ToString();
		}

	}
}
