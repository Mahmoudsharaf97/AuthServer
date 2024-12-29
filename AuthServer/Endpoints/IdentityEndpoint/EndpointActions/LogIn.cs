using Auth_Application.Features;
using Auth_Application.Models;
using IdentityApplication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Endpoints.IdentityEndpoint.EndpointActions
{
	public static class LogIn
	{
		public static async Task<LogInOutput> Action(IMediator _mediator, [FromBody] LogInInput input)
		{
			var request = MapperObject.Mapper.Map<BeginEndLoginQuery>(input);
			return await _mediator.Send(request);
		}
	}
}
