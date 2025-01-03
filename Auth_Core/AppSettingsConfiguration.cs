

namespace Auth_Core
{
    public class AppSettingsConfiguration 
    {
        public string AuthConnectionStringDB { get; set; }
        public int JwtTokenExpiryMinutes { get; set; }
        public int JwtRefreshTokenExpiryMinutes { get; set; }
        public string JwtSecretKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public int JwtSessionExpireInMinutes { get; set; }
        public bool RedisEnabled { get; set; }
        public string RedisConnectionString { get; set; }
        public int RedisInstance { get; set; }
        public string CaptchKey { get; set; }
        public string YakeenMobileVerifyUrl { get; set; }
        public int YakeenMobileVerifyTimeOutInsecond { get; set; }
        public string YakeenMobileVerifyAPPID { get; set; }
        public string YakeenMobileVerifyAPPKEY { get; set; }
        public string YakeenMobileVerifySERVICEKEY { get; set; }
        public string YakeenMobileVerifyORGANIZATIONNUMBER { get; set; }
        public string YakeenLocalURl { get; set; }
        public string YakeenLocalsvcCredentials { get; set; }

    }

}
