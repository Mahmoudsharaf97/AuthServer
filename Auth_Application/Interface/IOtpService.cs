using Auth_Application.Models;
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
		int GenerateRandomOTP();
		Task<OtpInfo> GetCachedOtpInfoAsync(string _phone);
		Task<bool> DeleteCachedOtpInfoAsync(string _phone);
	}
}
