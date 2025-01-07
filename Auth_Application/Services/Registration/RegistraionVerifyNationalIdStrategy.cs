
using Auth_Application.Features;
using Auth_Application.Interface;
using Auth_Application.Models;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using IdentityApplication.Models;
using static Auth_Application.Models.Errors;

namespace Auth_Application.Services.Registration
{
    public class RegistraionVerifyNationalIdStrategy : BaseRegistrationServices
    {
        private readonly IRedisCaching _redisCaching;
        private readonly IOtpService otpService;
        public IYakeenClient _yakeenClient { get; }
        public IYakeenNationalIdServices _yakeenNationalIdServices { get; }
        public RegistraionVerifyNationalIdStrategy(IRedisCaching redisCaching, IYakeenClient yakeenClient, IOtpService otpService,
            IApplicationUserManager applicationUserManager) : base(redisCaching, applicationUserManager)
        {
            _redisCaching = redisCaching;
            _yakeenClient = yakeenClient;
            this.otpService = otpService;
        }


        public async Task<RegisterOutPut> EndRegistration(RegisterCommand model)
        {
            RegisterOutPut result = new RegisterOutPut();
            try
            {
                // Get Begin Register Request  must be in Redis  
                var _cahedRegisterUser = await _redisCaching.GetRegisterUserAfterPhoneVerify(model.Email, model.NationalId.ToString(),model.Phone.ToString());
                if (_cahedRegisterUser is null || !_cahedRegisterUser.IsPhoneVerifiedByYakeen)
                {
                    result.Succes = false;
                    result.IsValidPhone = false;
                    result.errors.Add(new Error 
                    { ErrorCode=(int) Errors.ErrorCode.YakeenVerfyPhoneError,
                        ErrorDescription="Error Verify Phone Number" });
                    return result;
                }

                var _yakeenResult = await _yakeenNationalIdServices.GetUserDataFromYakeen(model.NationalId.ToString(), model.BirthDateYear.Value, model.BirthDateMonth.Value, model.Channel, model.Language);
                var userData = _yakeenResult.Item1;
                string  logException = _yakeenResult.Item2;
                string outputDescription = _yakeenResult.Item3;
                if (userData == null || !userData.IsExist || !string.IsNullOrEmpty(logException))
                {
                    result.Succes = false;
                    result.IsValidDateOfBirth = false;
                    result.errors.Add(new Error
                    {
                        ErrorCode = (int)Errors.ErrorCode.YakeenVerfiyDateOfBirthError,
                        ErrorDescription = "Invalid Date Of Birth "
                    });
                    //Output.Result.Hashed = string.Empty;
                    return result;
                }
                _cahedRegisterUser.DateOfBirthMonth = model.BirthDateMonth.ToString();
                _cahedRegisterUser.DateOfBirthYear = model.BirthDateYear.ToString();
                _cahedRegisterUser.Channel = model.Channel;
                _cahedRegisterUser.FullNameAr = userData.FullNameAr;
                _cahedRegisterUser.FullNameEn = userData.FullNameAr;

                OtpInfo otpInfo =  await otpService.SendOtp(SMSType.Register, _cahedRegisterUser);
                if (otpInfo is null )
                    return null;// need Handle
                await _redisCaching.SetRegisterUserAfterGenerateOTP(_cahedRegisterUser);
                result.OtpSend = true;
                result.Succes = true;
                result.errors = [];
                result.RegisterStatusCode = (int)RegisterStatusCode.DateOfBirthSuccessShowOTPRequired;
                return result;
            }
            catch (Exception ex)
            {
            result.Succes = false;
            return result;  
            }
        }

    }
}
