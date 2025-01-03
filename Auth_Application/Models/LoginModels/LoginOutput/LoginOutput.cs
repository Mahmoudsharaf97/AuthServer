using Auth_Core.Enums;
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
		public  override LoginMethod LoginMethod { get; set; }
		public override bool? PhoneNumberConfirmed { get; set; } = null;
		[JsonPropertyName("phoneNumber")]
		public override string PhoneNumber { get; set; }
		[JsonPropertyName("fnar")]
		public override string FullNameAr { get; set; }

		[JsonPropertyName("fnen")]
		public override string FullNameEn { get; set; }
		[JsonPropertyName("otp")]
		public override int OTP { get; set; }
		public override bool PhoneVerification { get; set; } = false; 
		[JsonPropertyName("nationalID")]
		public override string NationalID { get; set; }
	}
}
