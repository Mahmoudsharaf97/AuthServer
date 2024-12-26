using Auth_Application.Features;
using IdentityApplication.Features;
using MediatR;

namespace AuthServer.Endpoints.IdentityEndpoint.EndpointActions
{
	public static class GatToken
	{
		public static async Task<TokenResponse> Action(IMediator _mediator)
		{
			return await _mediator.Send(new GetAccessTokenQuery());
		}
	}
}
