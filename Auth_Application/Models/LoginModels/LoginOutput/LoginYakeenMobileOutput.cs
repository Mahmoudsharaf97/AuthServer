using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth_Application.Models.LoginModels.LoginOutput
{
	public class LoginYakeenMobileOutput : BaseLoginOutput
	{
		public  string PhoneNo { get; set; }
		[JsonPropertyName("fnar")]
		public  string FullNameAr { get; set; }

		[JsonPropertyName("fnen")]
		public  string FullNameEn { get; set; }
		[JsonPropertyName("otp")]
		public  int OTP { get; set; }
		[JsonPropertyName("isYakeenChecked")]
		public  bool IsYakeenChecked { get; set; }
		public bool? PhoneNumberConfirmed { get; set; } = null;
	}
}
