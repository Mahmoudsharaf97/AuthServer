using Auth_Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Features.Captcha.GetCaptcha
{
	public class CaptchaQuery : IRequest<CaptchaResponse>
	{
	}
}
