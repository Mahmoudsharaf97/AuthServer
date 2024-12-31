using Auth_Application.Models.Base;
using Auth_Core;
using Newtonsoft.Json;
using SME_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth_Application.Models.LoginModels
{
	public class VerifyLoginOTPModel : BaseLoginModel
	{
		public string Phone { get; set; }
		public int? OTP { get; set; }

		[JsonPropertyName("fnar")]
		public string FullNameAr { get; set; }

		[JsonPropertyName("fnen")]
		public string FullNameEn { get; set; }
		public override void ValidateModel()
		{
			base.ValidateModel();

			if (string.IsNullOrEmpty(Phone))
				throw new AppException(ExceptionEnum.MobileEmpty);
			if (!Utilities.IsValidPhoneNo(Phone))
				throw new AppException(ExceptionEnum.ErrorPhone);
			if (!OTP.HasValue)
				throw new AppException(ExceptionEnum.EmptyOTP);
			if (string.IsNullOrEmpty(FullNameAr))
				throw new AppException(ExceptionEnum.EmptyInputParameter);
			if (string.IsNullOrEmpty(FullNameEn))
				throw new AppException(ExceptionEnum.EmptyInputParameter);
		}

	}
}
