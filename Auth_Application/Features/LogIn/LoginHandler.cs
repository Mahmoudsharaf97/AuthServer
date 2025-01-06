using Auth_Application.Interface.Login;
using Auth_Application.Models;
using Auth_Application.Models.LoginModels.LoginOutput;
using MediatR;

namespace Auth_Application.Features.LogIn
{
	public class LoginHandler : IRequestHandler<LoginQuery, GenericOutput<BaseLoginOutput>>
	{
		private readonly ILoginStrategyManager _loginStrategyManager;
		public async Task<GenericOutput<BaseLoginOutput>> Handle(LoginQuery request, CancellationToken cancellationToken)
		{
			GenericOutput<BaseLoginOutput> output = await (_loginStrategyManager.GetStrategy(request.login.LoginMethod, request.login.LoginType)).Execute(request.login);
			return output;
		}
	}
}
