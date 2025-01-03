
using Auth_Application.Features;
using Auth_Core;
using Auth_Core.UseCase;
using Auth_Core.UseCase.Redis;
using IdentityApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth_Application.Services.Registration
{
    public class RegistraionVerifyNationalIdStrategy : BaseRegistrationServices
    {
        private readonly IRedisCaching _redisCaching;
        private readonly IApplicationUserManager applicationUserManager;
        private readonly UserManager<ApplicationUser<string>> userManager;

        public RegistraionVerifyNationalIdStrategy(IRedisCaching redisCaching, 
			IApplicationUserManager applicationUserManager, UserManager<ApplicationUser<string>> userManager) : base(redisCaching, applicationUserManager)
        {
            _redisCaching = redisCaching;
            this.applicationUserManager = applicationUserManager;
            this.userManager = userManager;
        }
        public async Task<RegisterOutPut> EndRegistration(RegisterCommand model)
        {
            try
            {
                // Get Begin Register Request 
                var _cahedRegisterUser = await _redisCaching.GetYakeenRegistUser(model.Email, model.NationalId.ToString());


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
