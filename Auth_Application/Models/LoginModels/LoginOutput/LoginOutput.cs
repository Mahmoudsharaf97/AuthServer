﻿using Auth_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth_Application.Models.LoginModels.LoginOutput
{
	public class LoginOutput : BaseLoginOutput
	{
		public   LoginMethod LoginMethod { get; set; }
		public  bool? PhoneNumberConfirmed { get; set; } = null;
		[JsonPropertyName("phoneNumber")]
		public  string PhoneNumber { get; set; }
		[JsonPropertyName("fnar")]
		public  string FullNameAr { get; set; }

		[JsonPropertyName("fnen")]
		public  string FullNameEn { get; set; }
		[JsonPropertyName("otp")]
		public  int OTP { get; set; }
		public  bool PhoneVerification { get; set; } = false; 
		[JsonPropertyName("nationalID")]
		public  string NationalID { get; set; }
	}
}
