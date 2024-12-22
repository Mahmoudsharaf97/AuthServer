using IdentityApplication.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Features
{
    public class LogOutHandler : IRequestHandler<LogOutCommand, bool>
    {
        public IIdentityServices _identityServices { get; }

        public LogOutHandler(IIdentityServices identityServices)
        {
            _identityServices = identityServices;
        }


        Task<bool> IRequestHandler<LogOutCommand, bool>.Handle(LogOutCommand request, CancellationToken cancellationToken)
        {
            return _identityServices.LogOut();
        }
    }
}
