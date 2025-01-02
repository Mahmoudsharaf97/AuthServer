

namespace Auth_Core
{
    public class AppSettingsConfiguration 
    {
        public bool SendOtpInResponse { get; set; }
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

    }

}
