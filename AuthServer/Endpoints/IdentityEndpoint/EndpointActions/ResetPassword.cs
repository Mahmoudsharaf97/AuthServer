using Auth_Application.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Endpoints.IdentityEndpoint.EndpointActions
{
	public static class ResetPassword
	{
		public static async Task<bool> Action(IMediator _mediator, [FromQuery] string email)
		{
			return await _mediator.Send(new ResetCommand(email));
		}
	}
}
