﻿using Auth_Application.Interface;
using Auth_Application.Interface.Login;
using Auth_Application.Models;
using Auth_Application.Models.LoginModels.LoginInput;
using Auth_Application.Models.LoginModels.LoginOutput;
using Auth_Application.Validations;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.Models.Yakeen;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Security.Cryptography.Xml;

namespace Auth_Application.Services.Login
{
	public abstract class VerifyLoginYakeenMobileService : LoginStrategy
	{
		private readonly IUsersCachedManager _usersCachedManager;
		private readonly IApplicationUserManager _applicationUserManager;
		private readonly IOtpService _otpService;
		private readonly IYakeenClient _yakeenClient;
		private readonly AppSettingsConfiguration _appSettings;
		public VerifyLoginYakeenMobileService(IUsersCachedManager usersCachedManager, IApplicationUserManager applicationUserManager, IOtpService otpService, AppSettingsConfiguration appSettings, IYakeenClient yakeenClient)
		{
			_usersCachedManager = usersCachedManager;
			_applicationUserManager = applicationUserManager;
			_otpService = otpService;
			_appSettings = appSettings;
			_yakeenClient = yakeenClient;
		}
		protected abstract Task ValidateUser(ApplicationUser<string> user, VerifyLoginYakeenMobile model);
		public override async Task<GenericOutput<BaseLoginOutput>> Execute(LoginInputModel loginInput)
		{
			GenericOutput<LoginYakeenMobileOutput> genericOutput = await VerifyLoginYakeenMobile(loginInput.VerifyLoginYakeenMobile);
			return new GenericOutput<BaseLoginOutput>()
			{
				ErrorDetails = genericOutput.ErrorDetails,
				Result = genericOutput.Result,
			};
		}
		public async Task<GenericOutput<LoginYakeenMobileOutput>> VerifyLoginYakeenMobile(VerifyLoginYakeenMobile model)// return base generic output
		{
			if (model is null)
				throw new AppException(ExceptionEnum.ModelIsEmpty);
			model.ValidateModel();

			GenericOutput<LoginYakeenMobileOutput> output = new();
			output.Result = new();

			ApplicationUser<string> user = await _usersCachedManager.GetUserByEmailOrNationalIdAsync(model.LoginType, model.UserName);
			await ValidateUser(user, model); // null, email confirmd, userLocked,
			bool IsEmailBelongsToOtherUser = await _applicationUserManager.CheckNationalIdBelongsForDifferentEmail(long.Parse( model.NationalId),user.Email);
			if (IsEmailBelongsToOtherUser)
				throw new AppException(ExceptionEnum.exist_nationalId_signup_error);
			// just this line not generic we should move it to the sub classes 
			await VerifyMobileByYakeen(output,user,long.Parse( model?.Phone) ,long.Parse( model?.NationalId) , "ar");

			CheckUserConfirmedByYakeen(output, user);
			if (output.Result.LoginMethod == LoginMethod.VerifyLoginOTP)
			{
				OtpInfo otpInfo = await _otpService.SendOtp(SMSType.LoginOtp, user);
				mapOutputModel(loginOutput: output.Result, user, otpInfo);
			}
			return output;
		}
		// move to base login 
		protected void CheckUserConfirmedByYakeen(GenericOutput<LoginYakeenMobileOutput> output, ApplicationUser<string> user)
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
			output.ErrorDetails = new(IsSuccess: false, ErrorCode: ExceptionEnum.success, ErrorDescription: "Login OTP is sended");
			output.Result.LoginMethod = LoginMethod.VerifyLoginOTP;
		}
		private void mapOutputModel(LoginYakeenMobileOutput loginOutput, ApplicationUser<string> user, OtpInfo otpInfo)
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
		private async Task VerifyMobileByYakeen(GenericOutput<LoginYakeenMobileOutput> output, ApplicationUser<string> user, long phone, long nationalId, string language)
		{
			YakeenMobileVerificationOutput yakeenMobileOutput = await _yakeenClient.YakeenMobileVerificationAsync(phone, nationalId,language);

			if (yakeenMobileOutput.ErrorCode == YakeenMobileVerificationOutput.ErrorCodes.InvalidMobileOwner)
			{
				output.ErrorDetails = new(IsSuccess: false, ErrorCode: ExceptionEnum.login_incorrect_phonenumber_not_verifed, ErrorDescription: "login incorrect phonenumber not verifed");
				output.Result.LoginMethod = LoginMethod.VerifyYakeenMobile;
			}
			else if (yakeenMobileOutput.ErrorCode != YakeenMobileVerificationOutput.ErrorCodes.Success)
			{
				output.ErrorDetails = new(IsSuccess: false, ErrorCode: ExceptionEnum.login_incorrect_phonenumber_not_verifed, ErrorDescription: "login incorrect phonenumber not verifed");
				output.Result.LoginMethod = LoginMethod.VerifyYakeenMobile;
			}

			user.IsPhoneVerifiedByYakeen = true;
			user.PhoneNumberConfirmed = true;
			user.PhoneNumber = phone.ToString();
			output.Result.LoginMethod = LoginMethod.Login;
		}
	}
}
