using MediatR;
namespace Auth_Application.Features
{
    public  class LogOutCommand : IRequest<bool>
    {
        public string Token { get; set; }
    }
}
