using Auth_Application.Features;
using Auth_Application.Features.LogIn;
using Auth_Application.Models;
using Auth_Application.Models.LoginModels.LoginInput;
using Auth_Application.Models.LoginModels.LoginOutput;
using IdentityApplication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Endpoints.IdentityEndpoint.EndpointActions
{
	public static class LogIn
	{
		public static async Task<GenericOutput<BaseLoginOutput>> Action(IMediator _mediator, [FromBody] LoginInputModel input)
		{
			return await _mediator.Send(new LoginQuery(input));
		}
	}
}
