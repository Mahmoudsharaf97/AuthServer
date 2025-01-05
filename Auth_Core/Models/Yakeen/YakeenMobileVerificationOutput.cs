namespace Auth_Core.Models.Yakeen
{
    public class YakeenMobileVerificationOutput
    {
        public enum ErrorCodes
        {
            Success = 1,
            NullResponse,
            UnspecifiedError,
            ServiceError,
            NullRequest,
            ServiceException,
            YakeenServiceException,
            DateOfBirthGIsEmpty,
            NoAddressFound,
            InternalError,
            InvalidId,
            InvalidMobileNumber,
            InvalidServiceKey,
            InvalidMobileOwner
        }
        public ErrorCodes ErrorCode
        {
            get;
            set;
        }
        public string ErrorDescription
        {
            get;
            set;
        }
        public MobileVerificationModel mobileVerificationModel { get; set; }
    }

    public class MobileVerificationModel
    {
        public string referenceNumber { get; set; }
        public string id { get; set; }
        public string mobile { get; set; }
        public bool isOwner { get; set; }
        public int code { get; set; }
        public string message { get; set; }
    }
}
