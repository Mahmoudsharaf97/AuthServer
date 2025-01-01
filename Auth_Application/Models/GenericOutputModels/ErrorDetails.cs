using Auth_Core.Enums;

namespace Auth_Application.Models
{
	public class ErrorDetails
	{
		public bool isSuccess { get; set; }
		public Guid referenceId { get; set; }
		public ErrorCodes errorCode { get; set; }
		public string errorTitle { get; set; }
		public string errorDescription { get; set; }
		public DateTime timestamp { get; set; }
		public ErrorDetails()
		{
			isSuccess = true;
			timestamp = DateTime.Now;
		}

		public ErrorDetails(bool isSuccess, ErrorCodes errorCode, string errorDescription)
		{
			this.isSuccess = isSuccess;
			this.referenceId = Guid.NewGuid();
			this.errorCode = errorCode;
			this.errorTitle = errorCode.ToString();
			this.errorDescription = errorDescription;
			this.timestamp = DateTime.Now;

		}

		public ErrorDetails(bool isSuccess, Guid referenceId, ErrorCodes errorCode, string errorDescription)
		{
			this.isSuccess = isSuccess;
			this.referenceId = referenceId;
			this.errorCode = errorCode;
			this.errorTitle = errorCode.ToString();
			this.errorDescription = errorDescription;
			this.timestamp = DateTime.Now;
		}
	}
}
