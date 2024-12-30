using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Interface
{
	public interface ICaptchService
	{
		Task<string> GenerateBase64Captcha(string captchaValue);
		bool ValidateCaptchaToken(string captchToken, string captchInput, string Key);
	}
}
