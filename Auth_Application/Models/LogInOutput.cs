

namespace Auth_Application.Models
{
    public class LogInOutput
    {
        public bool success { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? AccessTokenExpiration { get; set; }
    }
}
