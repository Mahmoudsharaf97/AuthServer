using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Models
{
	public class OtpInfo
	{
		public int Id { get; set; }

		public Guid UserId { get; set; }

		public string UserEmail { get; set; }

		public int VerificationCode { get; set; }

		public bool IsCodeVerified { get; set; }

		public string PhoneNumber { get; set; }

		public DateTime CreatedDate { get; set; }
		public DateTime ModifiedDate { get; set; }

		public string Nin { get; set; }

		public string SMSType { get; set; }

		/// <summary>
		/// UpdateProfileInfoOtp ProfileInfoTypeId
		/// </summary>
		public int ProfileInfoTypeId { get; set; }

		/// <summary>
		/// ForgotPasswordToken Token
		/// </summary>
		public string Token { get; set; }
		/// <summary>
		/// ForgotPasswordToken ForgotPasswordVerificationType
		/// </summary>
		public int ForgotPasswordVerificationType { get; set; }
	}
}
