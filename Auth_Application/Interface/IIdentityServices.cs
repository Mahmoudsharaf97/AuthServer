using Auth_Application.Features;
using Auth_Application.Models;
using Auth_Application.Models.LoginModels.LoginInput;
using Auth_Application.Models.LoginModels.LoginOutput;
using IdentityApplication.Models;

namespace IdentityApplication.Interface
{
    public interface IIdentityServices
    {
          Task<RegisterOutPut> Register(RegisterCommand model);
          Task<GenericOutput<BaseLoginOutput>> Login(LoginInputModel model);
          Task<IdentityOutput> GetToken(string userId, string SessionId);
          Task<bool> LogOut();
          string GenerateTokenJWT(string ID, string Email, string userName, string sessionId);
          Task<bool> ResetPassword(string email);

    }
}
