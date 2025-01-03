using Auth_Application.Features;
using Auth_Application.Models;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.Models;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using Auth_Core.UseCase.Yakeen;
using IdentityApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Auth_Application.Models.Errors;
namespace Auth_Application.Services.Registration
{
    public  class RegistraionVerifyPhoneStrategy : BaseRegistrationServices
    {
        private readonly IRedisCaching _redisCaching;
        private readonly IMobileVerifyService _mobileVerifyService;
        private readonly UserManager<ApplicationUser<string>> _userManager;

        public RegistraionVerifyPhoneStrategy(IRedisCaching redisCaching,
            IApplicationUserManager applicationUserManager, IMobileVerifyService mobileVerifyService,
            UserManager<ApplicationUser<string>> userManager) :base(redisCaching, applicationUserManager)
        {
            _redisCaching = redisCaching;
            ApplicationUserManager = applicationUserManager;
            _mobileVerifyService = mobileVerifyService;
            _userManager = userManager;
        }

        public IApplicationUserManager ApplicationUserManager { get; }

        public async Task<RegisterOutPut> BeginRegistration(RegisterCommand model)
        {
            RegisterOutPut YakeenResult = new RegisterOutPut();

            try
            {
                // check If Client Try To Begin Register Before 
                var _cahedRegisterUser =await  _redisCaching.GetYakeenRegistUser(model.Email, model.NationalId.ToString());

                if (_cahedRegisterUser is not null)
                {
                    if (!_cahedRegisterUser.IsPhoneVerifiedByYakeen)
                    {
                        YakeenResult.Succes = false;
                        YakeenResult.errors.Add(new Error { ErrorCode = (int)RegisterOutPut.ErrorCode.InvalidMobileOwner, ErrorDescription = "Phone Not Belong to National " });
                    }
                    else
                    {
                        YakeenResult.Succes = true;
                        YakeenResult.IsValidPhone = true;
                        YakeenResult.errors = [];
                    }
                   return YakeenResult;
                }

                RegisterOutPut outPut = await ValidateEmailExist(model.Email);
                if (!outPut.Succes)
                    return outPut;

  

                ApplicationUser<string> user = new ApplicationUser<string>
                {
                    Email = model.Email,
                    PasswordHash = model.Password,
                    UserName = model.Email,
                    PhoneNumberConfirmed = false,
                    EmailConfirmed = false,
                    NationalId=model.NationalId,
                    PhoneNumber=model.Phone.ToString(),
                    IsPhoneVerifiedByYakeen=false
                };

                // call Yakeen For Mobile Verifcation 
                YakeenMobileVerificationOutput res = await _mobileVerifyService.YakeenMobileVerificationAsync(model.Phone, model.NationalId, "ar");

                if (res.ErrorCode != YakeenMobileVerificationOutput.ErrorCodes.Success)
                {
                    if (res.ErrorCode== YakeenMobileVerificationOutput.ErrorCodes.InvalidMobileOwner)
                    {
                        YakeenResult.Succes = false ;
                        YakeenResult.errors.Add(new Error { ErrorCode = (int)RegisterOutPut.ErrorCode.InvalidMobileOwner, ErrorDescription = "Phone Not Belong to National " });
                    }
                    else
                    {
                        YakeenResult.Succes = false;
                        YakeenResult.errors.Add(new Error { ErrorCode = (int)RegisterOutPut.ErrorCode.YakeenVerfyPhoneError, ErrorDescription = "Error Happen Get Phone Number" });
                    }
                    return YakeenResult;
                }

                if (res.ErrorCode == YakeenMobileVerificationOutput.ErrorCodes.Success)
                {
                    user.IsPhoneVerifiedByYakeen = true;
                    await _redisCaching.SetYakeenRegisterUser(user);
                }


                YakeenResult.Succes = true;
                YakeenResult.IsValidPhone = true;
                YakeenResult.errors = [];
                return YakeenResult;
            }
            catch (Exception ex)
            {
                YakeenResult.Succes = false;
                YakeenResult.errors = [];
                return YakeenResult;
            }
        }

    }
}
