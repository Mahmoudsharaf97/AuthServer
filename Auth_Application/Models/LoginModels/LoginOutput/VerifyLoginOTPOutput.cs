namespace Auth_Application.Models.LoginModels.LoginOutput
{
	public class VerifyLoginOTPOutput : BaseLoginOutput
	{
		public string UserId { get; set; }
		public string Email { get; set; }
		public string AccessToken { get; set; }
		public string? RefreshToken { get; set; }
		//public string AccessTokenJwt { get; set; }
		public int TokenExpiryDate { get; set; }
		public string DisplayNameAr { get; set; }
		public string DisplayNameEn { get; set; }
		public DateTime AccessTokenExpiration { get; set; }

	}
}
