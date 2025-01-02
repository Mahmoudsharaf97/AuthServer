using Auth_Application.Interface;
using Auth_Application.Interface.Login;
using Auth_Application.Models;
using Auth_Application.Models.Base;
using Auth_Application.Models.LoginModels;
using Auth_Application.Models.LoginModels.LoginOutput;
using Auth_Application.Validations;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase.Redis;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Services.Login.NormalLogin
{
	public abstract class BaseNormalLoginService : ILoginStrategy
	{
		private readonly IUsersCachedManager _usersCachedManager;
		private readonly IOtpService _otpService;
		private readonly AppSettingsConfiguration _appSettings;

		protected BaseNormalLoginService()
		{
			
		}
		protected BaseNormalLoginService(IUsersCachedManager usersCachedManager, IOtpService otpService, AppSettingsConfiguration appSettings)
		{
			_usersCachedManager = usersCachedManager;
			_otpService = otpService;
			_appSettings = appSettings;
		}
		protected abstract Task ValidateUser(ApplicationUser<string> user, LoginModel model);
		public async Task<GenericOutput<LoginOutput>> Login(LoginModel model)
		{
			GenericOutput<LoginOutput> output = new();
			output.Result = new();
			if (model is null)
				throw new AppException(ExceptionEnum.ModelIsEmpty);
			model.ValidateModel();
			// check if user locked and count on login try 
			ApplicationUser<string> user = await _usersCachedManager.GetUserByEmailOrNationalIdAsync(model.LoginType, LoginType.NationalId == model.LoginType ? model.NationalId : model.UserName);
			await ValidateUser(user,model);
			CheckUserConfirmedByYakeen(output, user);
			if (output.Result.LoginMethod == LoginMethod.VerifyLoginOTP)
			{
				OtpInfo otpInfo = await _otpService.SendOtp(SMSType.LoginOtp, user);
				mapLoginOutput(loginOutput: output.Result, user, otpInfo);
			}
			return output;
		}
		protected void CheckUserConfirmedByYakeen(GenericOutput<LoginOutput> output ,ApplicationUser<string> user)
		{
			if (!user.IsYakeenNationalIdVerified())
			{
				output.ErrorDetails = new(IsSuccess: false, ErrorCode: ExceptionEnum.UserYakeenNationalIdNotVerified, ErrorDescription: "User Yakeen National Id Not Verified");
				output.Result.LoginMethod = LoginMethod.LoginAccountConfirmation;
			}
			if (!user.IsPhoneVerifiedByYakeen())
			{
				output.ErrorDetails = new(IsSuccess: false, ErrorCode: ExceptionEnum.login_incorrect_phonenumber_not_verifed, ErrorDescription: "login incorrect phonenumber not verifed");
				output.Result.LoginMethod = LoginMethod.VerifyYakeenMobile;
			}
			output.Result.LoginMethod = LoginMethod.VerifyLoginOTP;
		}

		private void mapLoginOutput(LoginOutput loginOutput, ApplicationUser<string> user, OtpInfo otpInfo)
		{
			loginOutput.PhoneNumberConfirmed = true;
			loginOutput.PhoneNumber = user.PhoneNumber;
			loginOutput.FullNameAr = user.FullNameAr;
			loginOutput.FullNameEn = user.FullNameEn;
			if(_appSettings.SendOtpInResponse)
				loginOutput.OTP = otpInfo.VerificationCode;
			loginOutput.PhoneVerification = true;
			loginOutput.NationalID = user.NationalId.ToString();

		}
	}
}
