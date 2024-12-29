using Auth_Application.Features.Token.AccessToken.Queries;
using Auth_Application.Features.Token.RefreshToken.Command;
using Auth_Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Endpoints.IdentityEndpoint.EndpointActions
{
	public class RefreshToken
	{
		public static async Task<IdentityOutput> Action(IMediator _mediator, [FromBody] RefreshTokenCommand refreshToken)
		{
			return await _mediator.Send(refreshToken);
		}
	}
}
