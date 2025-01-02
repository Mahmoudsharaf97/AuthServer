using Auth_Core.Enums;

namespace Auth_Application.Models
{
	public class ErrorDetails
	{
		public bool IsSuccess { get; set; }
		public Guid ReferenceId { get; set; }
		public ErrorCodes ErrorCode { get; set; }
		public string ErrorTitle { get; set; }
		public string ErrorDescription { get; set; }
		public DateTime Timestamp { get; set; }
		public ErrorDetails()
		{
			IsSuccess = true;
			Timestamp = DateTime.Now;
		}

		public ErrorDetails(bool IsSuccess, ErrorCodes ErrorCode, string ErrorDescription)
		{
			this.IsSuccess = IsSuccess;
			this.ReferenceId = Guid.NewGuid();
			this.ErrorCode = ErrorCode;
			this.ErrorTitle = ErrorCode.ToString();
			this.ErrorDescription = ErrorDescription;
			this.Timestamp = DateTime.Now;

		}

		public ErrorDetails(bool IsSuccess, Guid ReferenceId, ErrorCodes ErrorCode, string ErrorDescription)
		{
			this.IsSuccess = IsSuccess;
			this.ReferenceId = ReferenceId;
			this.ErrorCode = ErrorCode;
			this.ErrorTitle = ErrorCode.ToString();
			this.ErrorDescription = ErrorDescription;
			this.Timestamp = DateTime.Now;
		}
	}
}
