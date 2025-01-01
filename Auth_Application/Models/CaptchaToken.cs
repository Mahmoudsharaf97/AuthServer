
using System.Text.Json.Serialization;
namespace Auth_Application.Models
{
	public class CaptchaToken
	{
		public CaptchaToken(string captcha, DateTime expiryDate)
		{
			Captcha = captcha;
			ExpiryDate = expiryDate;
		}

		[JsonPropertyName("captcha")]
		public string Captcha { get; set; }
		[JsonPropertyName("expiryDate")]
		public DateTime ExpiryDate { get; set; }
	}
}
