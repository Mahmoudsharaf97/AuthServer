using Auth_Application.Models;
using Auth_Core;
namespace Auth_Application.Interface
{
    public  interface IUserSignInService
    {
        Task<LogInOutput> UserSignIn(ApplicationUser<string> user, string email, string password);
    }
}
