using Auth_Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth_Application.Models.LoginModels.LoginOutput
{
	public class BaseLoginOutput
	{
		public  LoginMethod LoginMethod { get; set; }
		public  bool? PhoneNumberConfirmed { get; set; } = null;
		public  string UserId { get; set; }
		public  bool RememberMe { get; set; }
		public  string Email { get; set; }
		public  bool IsCorporateUser { get; set; }
		public  bool IsCorporateAdmin { get; set; }
		public  string AccessToken { get; set; }
		public  string AccessTokenGwt { get; set; }
		public  int TokenExpiryDate { get; set; }

		[JsonPropertyName("phoneNumber")]
		public  string PhoneNumber { get; set; }
		public  string PhoneNo { get; set; }

		[JsonPropertyName("fnar")]
		public  string FullNameAr { get; set; }

		[JsonPropertyName("fnen")]
		public  string FullNameEn { get; set; }

		[JsonPropertyName("otp")]
		public  int OTP { get; set; }

		[JsonPropertyName("getBirthDate")]
		public  bool GetBirthDate { get; set; } = false;

		[JsonPropertyName("phoneVerification")]
		public  bool PhoneVerification { get; set; } = false;

		[JsonPropertyName("hashed")]
		public  string Hashed { get; set; }

		[JsonPropertyName("displayNameAr")]
		public  string DisplayNameAr { get; set; }

		[JsonPropertyName("displayNameEn")]
		public  string DisplayNameEn { get; set; }

		[JsonPropertyName("isYakeenChecked")]
		public  bool IsYakeenChecked { get; set; }

		[JsonPropertyName("tokenExpirationDate")]
		public  DateTime TokenExpirationDate { get; set; }
		[JsonPropertyName("nationalID")]
		public  string NationalID { get; set; }
	}
}
