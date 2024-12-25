
using Auth_Application.Models;

namespace Auth_Application.Interface
{
    public  interface ITokenServices
    {
        string GenerateTokenJWT(string ID, string Email, string userName, string sessionId);
        Task<IdentityOutput> GetToken(string userId, string SessionId);
    }
}
