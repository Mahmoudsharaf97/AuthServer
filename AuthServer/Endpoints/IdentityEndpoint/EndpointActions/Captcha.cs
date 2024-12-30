using Auth_Application.Features.Captcha.GetCaptcha;
using Auth_Application.Models;
using MediatR;

namespace AuthServer.Endpoints.IdentityEndpoint.EndpointActions
{
	public class Captcha
	{
		public static async Task<CaptchaResponse> Action(IMediator _mediator)
		{
			return await _mediator.Send(new CaptchaQuery());
		}
	}
}
