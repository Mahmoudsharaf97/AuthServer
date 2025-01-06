using Auth_Application.Models;
using Auth_Application.Models.LoginModels.LoginInput;
using Auth_Application.Models.LoginModels.LoginOutput;
using MediatR;

namespace Auth_Application.Features.LogIn
{
	public class LoginQuery : IRequest<GenericOutput<BaseLoginOutput>>
	{
		public LoginQuery(LoginInputModel login)
		{
			this.login = login;
		}
		public LoginInputModel login { get; set; }
	}
}
