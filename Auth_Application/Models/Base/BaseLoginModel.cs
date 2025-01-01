using Auth_Core;
using Auth_Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth_Application.Models.Base
{
	public class BaseLoginModel
	{
		[Required]
		[JsonPropertyName("loginType")]
		public LoginType LoginType { get; set; }
		[Required]
		[JsonPropertyName("loginMethod")]
		public LoginMethod LoginMethod { get; set; }
		public string UserName { get; set; }
		public string PWD { get; set; }
		public string NationalId { get; set; }
		[JsonPropertyName("birthMonth")]
		public int? BirthMonth { get; set; }
		[JsonPropertyName("birthYear")]
		public int? BirthYear { get; set; } 
		/// <summary>
		/// User captcha input.
		/// </summary>
		[JsonPropertyName("CaptchaInput")]
		public string CaptchaInput { get; set; }

		/// <summary>
		/// The captcha token.
		/// </summary>
		[JsonPropertyName("CaptchaToken")]
		public string CaptchaToken { get; set; }

		public virtual void ValidateModel()
		{
			if(LoginType.NationalId == this.LoginType)
			{
				if (string.IsNullOrEmpty(this.NationalId))
					throw new AppException(ExceptionEnum.NationalIdEmpty);
				if(!this.BirthYear.HasValue || this.BirthYear <= 0)
					throw new AppException(ExceptionEnum.ErrorBirthYear);
				if(!this.BirthMonth.HasValue || this.BirthMonth > 12)
					throw new AppException(ExceptionEnum.ErrorBirthYear);
			}
			else
			{
				if(string.IsNullOrEmpty(this.UserName))
					throw new AppException(ExceptionEnum.EmailIsEmpty);
				if(string.IsNullOrEmpty(this.PWD))
					throw new AppException(ExceptionEnum.PasswordIsEmpty);
			}
		}
	}
}
