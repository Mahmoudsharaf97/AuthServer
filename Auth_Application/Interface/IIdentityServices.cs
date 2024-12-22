using Auth_Application.Models;
using IdentityApplication.Models;

namespace IdentityApplication.Interface
{
    public interface IIdentityServices
    {
          Task<RegisterOutPut> Register(RegisterInput model);
          Task<LogInOutput> Login(LogInInput model);
          Task<IdentityOutput> GetToken(string userId, string SessionId);
          Task<bool> LogOut();
          string GenerateTokenJWT(string ID, string Email, string userName, string sessionId);
          Task<bool> ResetPassword(string email);

    }
}
