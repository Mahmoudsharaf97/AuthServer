using IdentityApplication.Models;
using MediatR;
namespace Auth_Application.Features
{
    public class RegisterCommand : IRequest<RegisterOutPut>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
