﻿using Auth_Application.Features;
using Auth_Application.Models;
using IdentityApplication.Models;

namespace IdentityApplication.Interface
{
    public interface IIdentityServices
    {
          Task<RegisterOutPut> Register(RegisterCommand model);
          Task<IdentityOutput> GetToken(string userId, string SessionId);
          Task<bool> LogOut();
          string GenerateTokenJWT(string ID, string Email, string userName, string sessionId);
          Task<bool> ResetPassword(string email);

    }
}
