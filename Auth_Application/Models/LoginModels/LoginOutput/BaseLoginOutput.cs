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
		public virtual LoginMethod LoginMethod { get; set; }
		public virtual bool? PhoneNumberConfirmed { get; set; } = null;
		public virtual string UserId { get; set; }
		public virtual bool RememberMe { get; set; }
		public virtual string Email { get; set; }
		public virtual bool IsCorporateUser { get; set; }
		public virtual bool IsCorporateAdmin { get; set; }
		public virtual string AccessToken { get; set; }
		public virtual string AccessTokenGwt { get; set; }
		public virtual int TokenExpiryDate { get; set; }

		[JsonPropertyName("phoneNumber")]
		public virtual string PhoneNumber { get; set; }
		public virtual string PhoneNo { get; set; }

		[JsonPropertyName("fnar")]
		public virtual string FullNameAr { get; set; }

		[JsonPropertyName("fnen")]
		public virtual string FullNameEn { get; set; }

		[JsonPropertyName("otp")]
		public virtual int OTP { get; set; }

		[JsonPropertyName("getBirthDate")]
		public virtual bool GetBirthDate { get; set; } = false;

		[JsonPropertyName("phoneVerification")]
		public virtual bool PhoneVerification { get; set; } = false;

		[JsonPropertyName("hashed")]
		public virtual string Hashed { get; set; }

		[JsonPropertyName("displayNameAr")]
		public virtual string DisplayNameAr { get; set; }

		[JsonPropertyName("displayNameEn")]
		public virtual string DisplayNameEn { get; set; }

		[JsonPropertyName("isYakeenChecked")]
		public virtual bool IsYakeenChecked { get; set; }

		[JsonPropertyName("tokenExpirationDate")]
		public virtual DateTime TokenExpirationDate { get; set; }
		[JsonPropertyName("nationalID")]
		public virtual string NationalID { get; set; }
	}
}
