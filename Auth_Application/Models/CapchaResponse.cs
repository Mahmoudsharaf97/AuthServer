using System.Text.Json.Serialization;

namespace Auth_Application.Models
{
    public class CaptchaResponse
	{
		public CaptchaResponse()
		{
			
		}
		public CaptchaResponse(string image, string token, int expiredInSeconds)
		{
			Image = image;
			Token = token;
			ExpiredInSeconds = expiredInSeconds;
		}

		/// <summary>
		/// The captcha image.
		/// </summary>
		[JsonPropertyName("image")]
        public string Image { get; set; }

        /// <summary>
        /// Captcha token.
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; }

        /// <summary>
        /// Captcah expiration in seconds.
        /// </summary>
        [JsonPropertyName("expiredInSeconds")]
        public int ExpiredInSeconds { get; set; }
		[JsonPropertyName("isValied")]
		public bool IsValied { get; set; }
	}
}
