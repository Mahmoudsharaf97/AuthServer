using Auth_Core;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using IdentityApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Auth_Application.Models.Errors;

namespace Auth_Application.Services.Registration
{
    public class BaseRegistrationServices
    {
        private readonly IRedisCaching _redisCaching;
        public IApplicationUserManager _applicationUserManager { get; }

        public BaseRegistrationServices(IRedisCaching redisCaching, IApplicationUserManager applicationUserManager)
        {
            _redisCaching = redisCaching;
            _applicationUserManager = applicationUserManager;
        }

        public async  Task<ApplicationUser<string>> EmailCachExist(string Email)
        {
          return   await  _redisCaching.GetUserAsync(Email);
        }
        public async  Task<ApplicationUser<string>> NationalIdCachExist(string nationalId)
        {
          return   await  _redisCaching.GetUserAsync(nationalId);
        }

        protected async  Task<RegisterOutPut> ValidateEmailExist(string email)
        {
            RegisterOutPut result = new RegisterOutPut();
            var cahedUser =await EmailCachExist(email);
            if (cahedUser is not null)
            {
                result.Succes = false;
                result.errors.Add(new Error
                {
                    ErrorCode = (int)ErrorCode.EmailAlreadyExist,
                    ErrorDescription = "Email Already Exist"
                });
                return result;
            }
            bool emailExist = await _applicationUserManager.CheckEmailExistAsync(email);
            if (emailExist)
            {
                result.Succes = false;
                result.errors.Add(new Error
                {
                    ErrorCode = (int)ErrorCode.EmailAlreadyExist,
                    ErrorDescription = "Email Already Exist"
                });
                return result;
            }


            result.Succes = true;
            result.errors.Add(new Error
            {
                ErrorCode = (int)ErrorCode.success,
                ErrorDescription = "Success"
            });
            return result;

        }
        protected async Task<RegisterOutPut> ValidateNationalIdExist(long nationalId)
        {
            RegisterOutPut result = new RegisterOutPut();
            var cahedUser = await NationalIdCachExist(nationalId.ToString());
            if (cahedUser is not null)
            {
                result.Succes = false;
                result.errors.Add(new Error
                {
                    ErrorCode = (int)ErrorCode.EmailAlreadyExist,
                    ErrorDescription = "NationalId Already Exist"
                });
                return result;
            }
            bool emailExist = await _applicationUserManager.CheckNationalIdExistAsync(nationalId);
            if (emailExist)
            {
                result.Succes = false;
                result.errors.Add(new Error
                {
                    ErrorCode = (int)ErrorCode.EmailAlreadyExist,
                    ErrorDescription = "Email Already Exist"
                });
                return result;
            }


            result.Succes = true;
            result.errors.Add(new Error
            {
                ErrorCode = (int)ErrorCode.success,
                ErrorDescription = "Success"
            });
            return result;

        }

        protected void ValidateCreateUser(IdentityResult createdUser)
        {
            if (!createdUser.Succeeded)
            {
                if (createdUser.Errors.ToList()[0].Code == "InvalidUserName")
                    throw new AppException(ExceptionEnum.InvalidUserName);

                if (createdUser.Errors.ToList()[0].Code == "InvalidEmail")
                    throw new AppException(ExceptionEnum.InvalidUserName);

                if (createdUser.Errors.ToList()[0].Code == "PasswordTooShort")
                    throw new AppException(ExceptionEnum.PasswordFormatNotValid);

                if (createdUser.Errors.ToList()[0].Code == "PasswordRequiresUpper")
                    throw new AppException(ExceptionEnum.PasswordRequiresUpper);

                if (createdUser.Errors.ToList()[0].Code == "PasswordRequiresNonAlphanumeric")
                    throw new AppException(ExceptionEnum.PasswordRequiresNonAlphanumeric);

                if (createdUser.Errors.ToList()[0].Code == "PasswordRequiresDigit")
                    throw new AppException(ExceptionEnum.PasswordRequiresDigit);

                throw new AppException(ExceptionEnum.RecordUpdateFailed);
            }
        }
       
    }
}
