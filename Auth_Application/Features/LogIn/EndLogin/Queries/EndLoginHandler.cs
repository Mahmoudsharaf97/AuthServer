using Auth_Application.Models;
using IdentityApplication;
using IdentityApplication.Interface;
using MediatR;

namespace Auth_Application.Features
{
	public class EndLoginHundler : IRequestHandler<EndLoginQuery, LogInOutput>
    {
        public IIdentityServices _identityServices { get; }
        public EndLoginHundler(IIdentityServices identityServices)
        {
            this._identityServices = identityServices;
        }


        public async Task<LogInOutput> Handle(EndLoginQuery request, CancellationToken cancellationToken)
        {
          var input = MapperObject.Mapper.Map<LogInInput>(request); // test success
            return await _identityServices.EndLogin(input);
        }
    }
}
