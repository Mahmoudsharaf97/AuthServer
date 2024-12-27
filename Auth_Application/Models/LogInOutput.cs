

namespace Auth_Application.Models
{
    public class LogInOutput
    {
		public LogInOutput()
		{
			
		}
		public LogInOutput(bool success, string? accessToken)
		{
            Success = success;
			AccessToken = accessToken;
		}
		public LogInOutput(bool success, string? accessToken, DateTime? accessTokenExpiration)
		{
			Success = success;
			AccessToken = accessToken;
			AccessTokenExpiration = accessTokenExpiration;
		}
		public bool Success { get; set; }
        public string? AccessToken { get; set; }
		public string? RefreshToken { get; set; }
        public DateTime? AccessTokenExpiration { get; set; }
    }
}
