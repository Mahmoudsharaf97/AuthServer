using Auth_Application.Models;
using Auth_Core;
using Auth_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Interface
{
	public interface IOtpService
	{
		Task<OtpInfo> SendOtp(SMSType smsType, ApplicationUser<string> user);
		int GenerateRandomOTP();
		Task<OtpInfo> GetCachedOtpInfoAsync(string _phone);
		Task<bool> DeleteCachedOtpInfoAsync(string _phone);
		Task<OtpInfo> SetOtpInfoAsync(ApplicationUser<string> user);
	}
}
