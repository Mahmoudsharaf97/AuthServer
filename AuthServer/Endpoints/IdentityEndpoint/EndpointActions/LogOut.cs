using Auth_Application.Features;
using MediatR;

namespace AuthServer.Endpoints.IdentityEndpoint.EndpointActions
{
	public static class LogOut
	{
		public static async Task<bool> Action(IMediator _mediator)
		{
			return await _mediator.Send(new LogOutCommand());
		}
	}
}
