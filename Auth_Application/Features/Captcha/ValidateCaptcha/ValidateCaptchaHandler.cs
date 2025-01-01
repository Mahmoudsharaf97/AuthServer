using Auth_Application.Interface;
using Auth_Application.Models;
using Auth_Core;
using Auth_Core.UseCase.Captch;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Features.Captcha.ValidateCaptcha
{
	internal class ValidateCaptchaHandler : IRequestHandler<ValidateCaptchaCommand, CaptchaResponse>
	{
		private readonly ICaptchService _captchService;
		private readonly AppSettingsConfiguration _appSettings;

		public ValidateCaptchaHandler(ICaptchService captchService, AppSettingsConfiguration appSettings)
		{
			_captchService = captchService;
			_appSettings = appSettings;
		}

		public async Task<CaptchaResponse> Handle(ValidateCaptchaCommand request, CancellationToken cancellationToken)
		{
			// input validations
			bool validCapcha = _captchService.ValidateCaptchaToken(request.CaptchaToken, request.CaptchaInput, _appSettings.JwtSecretKey);
			if (!validCapcha)
				throw new AppException(ExceptionEnum.WrongCapcha);

			return new CaptchaResponse { IsValied = validCapcha };
		}
	}
}
