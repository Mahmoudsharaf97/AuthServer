using Auth_Application.Interface;
using Auth_Application.Models;
using Auth_Core.Enums;
using Auth_Core.Helper;
using Auth_Core.UseCase.Redis;
using SME_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Application.Services.Otp
{
	internal class OtpService : IOtpService
	{
		private readonly IRedisCaching _redisCaching;

		public OtpService(IRedisCaching redisCaching)
		{
			_redisCaching = redisCaching;
		}

		public int GenerateRandomOTP()
		{
			return new Random().Next(1000, 9999);
		}
		public async Task<OtpInfo> GetCachedOtpInfoAsync(string _phone)
		{
			var cacheKey = $"OTP_{AppPoolHelper.APP_POOL_NAME.Trim()}_{Utilities.ValidatePhoneNumber(_phone)}";
			OtpInfo otpInfo = await _redisCaching.GetAsync<OtpInfo>(cacheKey);

			if (otpInfo == null)
				return null;

			return otpInfo;
		}
		public async Task<bool> DeleteCachedOtpInfoAsync(string _phone)
		{
			var cacheKey = $"OTP_{AppPoolHelper.APP_POOL_NAME.Trim()}_{Utilities.ValidatePhoneNumber(_phone)}";
			return await _redisCaching.DeleteAsync(cacheKey);
		}
	}
}
