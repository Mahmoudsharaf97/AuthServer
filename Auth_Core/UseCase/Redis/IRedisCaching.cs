﻿using Newtonsoft.Json;

namespace Auth_Core.UseCase.Redis
{
    public interface IRedisCaching
    {
        Task<T> GetAsync<T>(string key);
        Task<bool> DeleteAsync(string key);
        Task<string> GetRefreshTokenAsync(string email);
        Task<bool> SetRefreshTokenAsync(string userId, string tokenValue);
        Task<bool> SetSessionAsync(string userId, SessionStatus session);
        Task<SessionStatus> GetSessionAsync(string email);
        Task<bool> DeleteSessionAsync(string userId);
       Task< bool> SetUser(string email, string ninKey, ApplicationUser<string> user);
        //bool SetUserAsync(string email,ApplicationUser<string> user);
        Task<ApplicationUser<string>> GetUserAsync(string email, bool IsEmail = false, bool isNin = false);
        Task<bool> SetAsync<T>(string key, T value);
        Task<bool> SetRegisterUserAfterPhoneVerify(ApplicationUser<string> user);
        Task<ApplicationUser<string>> GetRegisterUserAfterPhoneVerify(string userEmail, string userNin, string phone);
        Task<bool> SetRegisterUserAfterGenerateOTP(ApplicationUser<string> user);
        Task<ApplicationUser<string>> GetRegisterUserAfterGenerateOTP(string userEmail, string userNin, string phone);
        Task<bool> DeletUserRegisterTries(string userEmail, string userNin, string phone);
    }
     
}
