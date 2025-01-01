
using Auth_Application.Features;
using Auth_Core;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using IdentityApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth_Application.Services.Registration
{
    public class RegistraionYakeenStrategy : BaseRegistrationServices
    {
        private readonly IRedisCaching redisCaching;
        private readonly IApplicationUserManager applicationUserManager;
        private readonly UserManager<ApplicationUser<string>> userManager;

        public RegistraionYakeenStrategy(IRedisCaching redisCaching, 
			IApplicationUserManager applicationUserManager, UserManager<ApplicationUser<string>> userManager) : base(redisCaching, applicationUserManager)
        {
            this.redisCaching = redisCaching;
            this.applicationUserManager = applicationUserManager;
            this.userManager = userManager;
        }
        public async Task<RegisterOutPut> YakeenRegistration(RegisterCommand model)
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
                    NationalId = model.NationalId
                };
                var createdUser = await userManager.CreateAsync(user, model.Password);
                ValidateCreateUser(createdUser);
                redisCaching.SetUser(user.Email, user.NationalId.ToString(), user);
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
