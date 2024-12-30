using Auth_Application.Features.Captcha.GetCaptcha;
using Auth_Application.Features.Captcha.ValidateCaptcha;
using Auth_Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Endpoints.IdentityEndpoint.EndpointActions
{
	public class ValidateCaptcha
	{
		public static async Task<CaptchaResponse> Action(IMediator _mediator, [FromBody] ValidateCaptchaCommand request)
		{
			return await _mediator.Send(request);
		}
	}
}
