using Auth_Application.Models;
using IdentityApplication;
using IdentityApplication.Interface;
using IdentityApplication.Models;
using MediatR;

namespace Auth_Application.Features
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterOutPut>
    {
        public IIdentityServices _identityServices { get; }
        public RegisterHandler(IIdentityServices identityServices)
        {
            _identityServices = identityServices;
        }
        public async Task<RegisterOutPut> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            //var input = MapperObject.Mapper.Map<RegisterInput>(request);
            var result= await _identityServices.Register(request);
            return result;
        }
    }
}
