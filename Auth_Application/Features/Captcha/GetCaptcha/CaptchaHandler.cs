using Auth_Application.Interface;
using Auth_Application.Models;
using Auth_Core;
using Auth_Core.Helper;
using Auth_Core.UseCase.Captch;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Features.Captcha.GetCaptcha
{
	internal class CaptchaHandler : IRequestHandler<CaptchaQuery, CaptchaResponse>
	{
		private readonly ICaptchService _captchService;
		private readonly AppSettingsConfiguration _appSettings;

		public CaptchaHandler(ICaptchService captchService, AppSettingsConfiguration appSettings)
		{
			_captchService = captchService;
			_appSettings = appSettings;
		}

		public async Task<CaptchaResponse> Handle(CaptchaQuery request, CancellationToken cancellationToken)
		{
			int num = new Random().Next(1000, 9999);
			string str = await _captchService.GenerateBase64Captcha(num.ToString());
			string img = $"data:image/jpeg;base64,{str}";
			var captchaToken = new CaptchaToken(captcha: num.ToString(), expiryDate: DateTime.Now.AddSeconds(600));
			string token = AESEncryptionUtilities.EncryptString(JsonConvert.SerializeObject(captchaToken), _appSettings.JwtSecretKey);
			return new CaptchaResponse(image: img, token, expiredInSeconds: 600);
		}
	}
}
