using Auth_Application.Interface.Login;
using Auth_Application.Models;
using Auth_Application.Models.LoginModels.LoginOutput;
using IdentityApplication.Interface;
using MediatR;

namespace Auth_Application.Features.LogIn
{
	public class LoginHandler : IRequestHandler<LoginQuery, GenericOutput<BaseLoginOutput>>
	{
		private readonly IIdentityServices _identityServices;

		public LoginHandler(IIdentityServices identityServices)
		{
			_identityServices = identityServices;
		}

		public async Task<GenericOutput<BaseLoginOutput>> Handle(LoginQuery request, CancellationToken cancellationToken)
		{
			return await _identityServices.Login(request.login);
		}
	}
}
