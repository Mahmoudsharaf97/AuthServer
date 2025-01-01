﻿namespace Auth_Core
{
    public enum ExceptionEnum
    {

		GenricError = -1,
		success = 0,
		RecordNotExist = 1,
		ModelNotValid = 2,
		NotAuthorized = 3,
		PropertyNotAccess = 4,
		WrongCredentials = 5,
		RecordCannotBeDelete = 6,
		RecordAlreadyExist = 7,
		RecordCreationFailed = 8,
		RecordUpdateFailed = 9,
		RecordDeleteFailed = 10,
		RecordNameAlreadyExist = 11,
		RecordEmailAlreadyExist = 12,
		TenancyNameAlreadyExist = 13,
		TenantExpected = 14,
		TenantNotZero = 15,
		AttachmentsRequired = 16,
		CouldNotMoveFiles = 17,
		MapperIssue = 18,
		InCorrectFileLength = 19,
		PhoneNumberAlreadyExist = 20,
		EmailAlreadyExist = 21,
		ClientIdAlreadyExist = 22,
		YakeenClientNotFound = 23,
		DakhleeClientNotFound = 24,
		OtpNotFound = 25,
		OtpNotMath = 26,
		OtpExpired = 27,
		SendMailFailed = 28,
		SendSMSFailed = 29,
		EmailExpired = 30,
		UserNotFound = 31,
		EmailAlreadyConfirmed = 32,
		PasswordAlreayUsed = 33,
		KeyNotValid = 34,
		UserIsLocked = 35,
		UserNotActiveFor3Months = 36,
		UserPhoneNotActiveConfirmed = 37,
		UserEmailNotconfirmed = 38,
		UserLoginDataNotCorrect = 39,
		FailedGenerateUserSession = 40,
		EmptyResponse = 41,
		SmsSettingsNotExist = 42,
		YakeenAddressNotFound = 43,
		PhoneNumberNotCorrect = 44,
		PasswordFormatNotValid = 45,
		PasswordRequiresUpper = 46,
		PasswordRequiresNonAlphanumeric = 47,
		PasswordRequiresDigit = 48,
		DisclaimerError = 49,
		OTPAfterOneMinute = 50,
		PhoneNoNotBelongToUser = 51,
		FileNotExist = 52,
		YaqeenGetPlateInfo = 53,
		YaqeenGetVehicleInf = 54,
		ModelValidationError = 55,
		InvalidVehiclsExcelSheet = 56,
		InvalidMakerCode = 57,
		PolicyEffectiveDate = 58,
		FailSaveFileOnServer = 60,
		InvalidCaptchInput = 61,
		invalidProviderID = 62,
		NoProductToShow = 63,
		ErrorHappenGetData = 64,
		UserDeleted = 65,
		InvalidUserName = 66,
		WrongHashing = 67,
		ErrorGetShoppingCart = 68,
		ExpireQuotation = 69,
		ErrorRequestPolicyEffectiveDate = 70,
		NoCrInfoDataForThisNumber = 71,
		WrongCapcha = 72,
		ServiceWithoutInsuranceCompany = 73,
		InvalidModelCode = 74,
		InValidInquiryLoginOtp = 75,
		InValidPhoneNumber = 76,
		InValidNotificationTypes = 77,
		InValidUserIp = 78,
		SendNotificationsFailed = 79,
		InValidCheckoutOtpNotification = 80,
		EmailNotValid = 81,
		CachedRefreshTokenWithEmailNotFound = 82,
		RefreshTokenIsWrong = 83,
		NationalIdEmpty = 84,
		ErrorBirthYear = 85,
		EmailIsEmpty = 86,
		PasswordIsEmpty = 87,
		MobileEmpty = 88,
		ErrorPhone = 89,
		EmptyOTP = 90,
		EmptyInputParameter = 91,
		NoAcouuntToId = 92,
		login_incorrect_email_message = 93,
		ErrorOTPCompare = 94,
		ErrorOTPExpire = 95,
		ModelIsEmpty = 96,
		WrongLoginMethod = 97,
	}

}

