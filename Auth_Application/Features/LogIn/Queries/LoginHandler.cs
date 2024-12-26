using Auth_Application.Models;
using IdentityApplication;
using IdentityApplication.Interface;
using MediatR;

namespace Auth_Application.Features
{
	public class LoginHundler : IRequestHandler<LoginQuery, LogInOutput>
    {
        public IIdentityServices _identityServices { get; }
        public LoginHundler(IIdentityServices identityServices)
        {
            _identityServices = identityServices;
        }


        public async Task<LogInOutput> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
          var input = MapperObject.Mapper.Map<LogInInput>(request); // test success
            return await _identityServices.Login(input);
        }
    }
}
