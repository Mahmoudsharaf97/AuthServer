﻿using Auth_Core;
using Auth_Core.UseCase;
using Auth_Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
namespace Auth_Infrastructure.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser<string>>, IApplicationUserManager
    {
        private UserStore<ApplicationUser<string>, ApplicationRole<string>, AuthContext, string, IdentityUserClaim<string>
            , IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>
            , IdentityRoleClaim<string>>
            _store;
        public ApplicationUserManager(IUserStore<ApplicationUser<string>> store,
            IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser<string>> passwordHasher, IEnumerable<IUserValidator<ApplicationUser<string>>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser<string>>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser<string>>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
     
        }
        private AuthContext GetContext()
        {
            _store = (UserStore<ApplicationUser<string>, ApplicationRole<string>, AuthContext, string, IdentityUserClaim<string>,
                    IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>)Store;

            var context = _store.Context;
            return context;
        }


        public async Task<bool> CheckEmailExistAsync(string email)
        {
            var context = GetContext();
            return await context.Users.AnyAsync(x => x.Email == email.ToLower());
        }
        public async Task<bool> UserIsExistAsync(string Id)
        {
            var context = GetContext();
            return await context.Users.AnyAsync(x => x.Id == Id);
        }
        public async Task<ApplicationUser<string>> GetUserByIdAsync(string Id)
        {
            var context = GetContext();
            return await context.Users.FirstOrDefaultAsync(x => x.Id == Id);
        }
     
        public async Task<string> GetUserNameByIdAsync(string Id)
        {
            var context = GetContext();
            var user= await context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            return user?.UserName;
        }
        public async Task<ApplicationUser<string>> GetUserByEmailAsync(string email)
        {
            var context = GetContext();
            return await context.Users.FirstOrDefaultAsync(x => x.Email == email.ToLower() );
        }
        public async Task<ApplicationUser<string>> GetUserByEmail(string email)
        {
            var context = GetContext();
            return await context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> IsBloked(string userId)
        {
           var user = await GetUserByIdAsync(userId);
            if (user == null && user.LockoutEnabled)
                return true;
            else
                return false;
        }

    

    }
    public class Fake_SME_DbContext
    {

    }
}
