using MediatR;

namespace Auth_Application.Features.Token.AccessToken.Queries
{
	public class GetAccessTokenQuery : IRequest<TokenResponse>
	{
	}

	public class TokenResponse
	{
		public string? Token { get; set; }
		public byte? ErrorCode { get; set; }
		public string? ErrorDescription { get; set; }
		public long? Expires_in { get; set; }
		public bool? CanPurchase { get; set; }
		public DateTime? TokenExpirationDate { get; set; }
	}
}