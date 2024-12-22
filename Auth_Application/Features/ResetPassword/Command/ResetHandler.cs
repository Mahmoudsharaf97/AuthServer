using MediatR;
using IdentityApplication.Interface;

namespace Auth_Application.Features
{
    internal class ResetHandler : IRequestHandler<ResetCommand, bool>
    {
        public IIdentityServices _identityServices { get; }
        public ResetHandler(IIdentityServices identityServices )
        {
            _identityServices = identityServices;
        }

        public async Task<bool> Handle(ResetCommand request, CancellationToken cancellationToken)
        {
          return await _identityServices.ResetPassword(request.Email);
     
        }
    }
}
