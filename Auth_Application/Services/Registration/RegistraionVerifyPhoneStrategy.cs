using Auth_Application.Features;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.Models.Yakeen;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using IdentityApplication.Models;
using Microsoft.AspNetCore.Identity;
using static Auth_Application.Models.Errors;
namespace Auth_Application.Services.Registration
{
    public  class RegistraionVerifyPhoneStrategy : BaseRegistrationServices
    {
        private readonly IRedisCaching _redisCaching;
        private readonly IYakeenClient _yakeenClient;

        public RegistraionVerifyPhoneStrategy(IRedisCaching redisCaching,
            IApplicationUserManager applicationUserManager, IYakeenClient yakeenClient
          ) :base(redisCaching, applicationUserManager)
        {
            _redisCaching = redisCaching;
            _yakeenClient = yakeenClient;
        }
        public async Task<RegisterOutPut> BeginRegistration(RegisterCommand model)
        {
            RegisterOutPut YakeenResult = new RegisterOutPut();
            try
            {
                // check If Client Try To Begin Register Before 
                var _cahedRegisterUser =await  _redisCaching.GetRegisterUserAfterPhoneVerify(model.Email, model.NationalId.ToString(),model.Phone.ToString());

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

                RegisterOutPut emailValidationOutPut = await ValidateEmailExist(model.Email);
                if (!emailValidationOutPut.Succes)
                    return emailValidationOutPut;

                RegisterOutPut nationalIdValidationOutPut = await ValidateNationalIdExist(model.NationalId.Value);
                if (!nationalIdValidationOutPut.Succes)
                    return nationalIdValidationOutPut;

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
                YakeenMobileVerificationOutput res = await _yakeenClient.YakeenMobileVerificationAsync(model.Phone.Value, model.NationalId.Value, model.Language);

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
                    await _redisCaching.SetRegisterUserAfterPhoneVerify(user);
                }
                YakeenResult.Succes = true;
                YakeenResult.IsValidPhone = true;
                YakeenResult.errors = [];
                YakeenResult.RegisterStatusCode =(int)RegisterStatusCode.PhoneVerfiedSuccssShowDateOfBirhRequired;
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
