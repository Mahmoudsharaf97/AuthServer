using Auth_Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth_Application.Features.Captcha.ValidateCaptcha
{
	public class ValidateCaptchaCommand : IRequest<CaptchaResponse>
	{
		[JsonPropertyName("captchaInput")]
		public string CaptchaInput { get; set; }
		[JsonPropertyName("captchaToken")]
		public string CaptchaToken { get; set; }
	}
}
