using Auth_Application.Models;
using MediatR;

namespace Auth_Application.Features
{
    public class LoginQuery : IRequest<LogInOutput>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
