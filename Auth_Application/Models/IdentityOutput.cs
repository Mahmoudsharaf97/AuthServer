using Auth_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Models
{
    public class IdentityOutput
    {
		public IdentityOutput()
		{
			
		}
		public IdentityOutput(LogInOutput result,ErrorCodes errorCode, int? tokenExpiryMinutes, string email, DateTime? tokenExpiryDate, string userName)
		{
			Result = result;
			ErrorCode = errorCode;
			TokenExpiryMinutes = tokenExpiryMinutes;
			Email = email;
			TokenExpiryDate = tokenExpiryDate;
			UserName = userName;
		}

        /// <summary>
        /// ErrorDescription
        /// </summary>
        public string ErrorDescription { get; set; }

        /// <summary>
        /// ErrorCode
        /// </summary>
        public ErrorCodes ErrorCode { get; set; }

		/// <summary>
		/// Result
		/// </summary>
		public LogInOutput Result { get; set; }

        public int ? TokenExpiryMinutes { get; set; }
        public string  Email { get; set; }
        public string  UserName { get; set; }
        public DateTime ? TokenExpiryDate { get; set; }
 
    }
}
