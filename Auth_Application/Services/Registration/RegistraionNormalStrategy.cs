using Auth_Application.Features;
using Auth_Application.Models;
using Auth_Core;
using Auth_Core.Enums;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using IdentityApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Auth_Application.Models.Errors;
namespace Auth_Application.Services.Registration
{
    public  class RegistraionNormalStrategy : BaseRegistrationServices
    {
        private readonly IRedisCaching _redisCaching;
        private readonly UserManager<ApplicationUser<string>> _userManager;

        public RegistraionNormalStrategy(IRedisCaching redisCaching,
            IApplicationUserManager applicationUserManager,
            UserManager<ApplicationUser<string>> userManager) :base(redisCaching, applicationUserManager)
        {
            _redisCaching = redisCaching;
            ApplicationUserManager = applicationUserManager;
            _userManager = userManager;
        }

        public IApplicationUserManager ApplicationUserManager { get; }

        public async Task<RegisterOutPut> NormalRegistration(RegisterCommand model)
        {
            try
            {
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
                    NationalId=model.NationalId
                };
                var createdUser = await _userManager.CreateAsync(user, model.Password);
                  ValidateCreateUser(createdUser);
                 _redisCaching.SetUser(user.Email, user.NationalId.ToString(), user);
                RegisterOutPut result = new RegisterOutPut
                {
                    Succes = true,
                    errors = []
                };
                return result;
            }
            catch (Exception ex)
            {
                RegisterOutPut result = new RegisterOutPut
                {
                    Succes = false
                };
                return result;
            }
        }

    }
}
