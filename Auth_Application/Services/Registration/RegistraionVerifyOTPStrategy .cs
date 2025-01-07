
using Auth_Application.Features;
using Auth_Application.Interface;
using Auth_Application.Models;
using Auth_Application.Services.Token;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using IdentityApplication.Models;
using Microsoft.AspNetCore.Identity;
using SME_Core;
using static Auth_Application.Models.Errors;

namespace Auth_Application.Services.Registration
{
    public class RegistraionVerifyOTPStrategy : BaseRegistrationServices
    {
        private readonly IRedisCaching _redisCaching;
        private readonly IOtpService otpService;
        private readonly IApplicationUserManager applicationUserManager;
        private readonly UserManager<ApplicationUser<string>> _userManager;
        public IYakeenClient _yakeenClient { get; }
        public IYakeenNationalIdServices _yakeenNationalIdServices { get; }
        public IUserSignInService _userSignInService { get; }

        public RegistraionVerifyOTPStrategy(IRedisCaching redisCaching, IOtpService otpService,
            IApplicationUserManager applicationUserManager, 
            UserManager<ApplicationUser<string>> userManager, IUserSignInService userSignInService) 
            : base(redisCaching, applicationUserManager)
        {
            _redisCaching = redisCaching;
            this.otpService = otpService;
            this.applicationUserManager = applicationUserManager;
            this._userManager = userManager;
            _userSignInService = userSignInService;
        }

        public async Task<RegisterOutPut> RegistrationVerifyOTP(RegisterCommand model)
        {
            RegisterOutPut result = new RegisterOutPut();
            try
            {
                OtpInfo otpInfo = await otpService.GetCachedOtpInfoAsync(model.Phone.ToString());
                if (otpInfo is null || otpInfo.VerificationCode!=model.OTP)
                    return null;// need To handle Error 
                await otpService.DeleteCachedOtpInfoAsync(model.Phone.ToString());
               result= await ValidateAndDeleteIfPhoneWithOterUser(model.Phone.ToString());
                if (!result.Succes)
                    return result;
                //get Register User From Redis 
                ApplicationUser<string> applicationUser = await _redisCaching.
                    GetRegisterUserAfterGenerateOTP(model.Email, model.NationalId.ToString(), model.Phone.ToString());
                if (applicationUser is null )
                {
                    result.Succes = false;
                    result.errors.Add(new Error { ErrorCode = 222, ErrorDescription = "faile Get user From Redis " });
                    return result;
                }
                ApplicationUser<string> user = HandleUserBeforInsert(applicationUser,model.Channel);
                var createdUser = await _userManager.CreateAsync(HandleUserBeforInsert(applicationUser,model.Channel), applicationUser.PasswordHash);
                ValidateCreateUser(createdUser);
                await _redisCaching.SetUser(user.Email, user.NationalId.ToString(), user);
                await _redisCaching.DeletUserRegisterTries(user.Email, user.NationalId.ToString(), user.PhoneNumber);
                result.OtpSend = true;
                result.Succes = true;
                result.errors = [];
                result.LogInOutput = await _userSignInService.UserSignIn(user, user.Email, applicationUser.PasswordHash);
                result.RegisterStatusCode = (int)RegisterStatusCode.Success;
                return result;
            }
            catch (Exception ex)
            {
            result.Succes = false;
            return result;  
            }
        }

        private ApplicationUser<string> HandleUserBeforInsert(ApplicationUser<string> applicationUser ,string channel)
        {
            applicationUser.CreatedDate = DateTime.Now;
            applicationUser.LastModifiedDate = DateTime.Now;
            applicationUser.LastLoginDate = DateTime.Now;
            applicationUser.LockoutEndDateUtc = DateTime.UtcNow.AddDays(-1);
            applicationUser.DeviceToken = "";
            applicationUser.EmailConfirmed = true;
            applicationUser.RoleId = Guid.Parse("DB5159FA-D585-4FEE-87B1-D9290D515DFB");
            applicationUser.LanguageId = Guid.Parse("5046A00B-D915-48A1-8CCF-5E5DFAB934FB");
            applicationUser.PhoneNumber = Utilities.ValidateInternalPhoneNumber(applicationUser.PhoneNumber);
            applicationUser.PhoneNumberConfirmed = true;
            applicationUser.TwoFactorEnabled = false;
            applicationUser.Channel = channel;
            return applicationUser;
        }

        private async  Task< RegisterOutPut> ValidateAndDeleteIfPhoneWithOterUser(string? _phone)
        {
            RegisterOutPut outPut=new RegisterOutPut(); 
            ApplicationUser<string> userInfo = await applicationUserManager
                 .GetUserByPhoneFormate(Utilities.ValidateInternalPhoneNumber(_phone),
                                        Utilities.ValidatePhoneNumber(_phone));
            if (userInfo != null)
            {
                userInfo.PhoneNumber = null;
                userInfo.PhoneNumberConfirmed = false;
                userInfo.IsPhoneVerifiedByYakeen = false;
                //userInfo.NationalId = null;
                var updateUserInfo =await _userManager.UpdateAsync(userInfo);
                if (!updateUserInfo.Succeeded)
                {
                    outPut.Succes=false;
                    outPut.errors.Add(new Error { ErrorCode = 11, ErrorDescription = "fail to Update" });
                    return outPut;
                }

            }
            outPut.Succes = true;
            return outPut;
        }

       
    }
}
