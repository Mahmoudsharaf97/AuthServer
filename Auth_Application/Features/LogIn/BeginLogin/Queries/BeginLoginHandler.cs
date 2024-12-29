using Auth_Application.Models;
using IdentityApplication;
using IdentityApplication.Interface;
using MediatR;

namespace Auth_Application.Features
{
	public class BeginLoginHundler : IRequestHandler<BeginLoginQuery, LogInOutput>
    {
        public IIdentityServices _identityServices { get; }
        public BeginLoginHundler(IIdentityServices identityServices)
        {
            _identityServices = identityServices;
        }


        public async Task<LogInOutput> Handle(BeginLoginQuery request, CancellationToken cancellationToken)
        {
          var input = MapperObject.Mapper.Map<LogInInput>(request); // test success
            return await _identityServices.BeginLogin(input);
        }
    }
}
