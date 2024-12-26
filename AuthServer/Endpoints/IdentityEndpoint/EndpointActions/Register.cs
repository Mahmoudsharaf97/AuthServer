using Auth_Application.Features;
using Auth_Application.Models;
using IdentityApplication;
using IdentityApplication.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Endpoints.IdentityEndpoint.EndpointActions
{
	public static class Register
	{
		public static async Task<RegisterOutPut> Action(IMediator _mediator,[FromBody] RegisterInput input)
		{
			RegisterCommand request = MapperObject.Mapper.Map<RegisterCommand>(input);
			return await _mediator.Send(request);
		}
	}
}
