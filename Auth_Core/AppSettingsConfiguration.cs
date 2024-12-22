

namespace Auth_Core
{
    public class AppSettingsConfiguration 
    {
        public string AuthConnectionStringDB { get; set; }
        public int JwtTokenExpiryMinutes { get; set; }
        public string JwtSecretKey { get; set; }
        public bool RedisEnabled { get; set; }
        public string RedisConnectionString { get; set; }
        public int RedisInstance { get; set; }

    }

}
