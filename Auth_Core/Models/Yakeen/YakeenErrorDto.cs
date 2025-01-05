
namespace Auth_Core.UseCase
{
    public class YakeenErrorDto
    {
        public EErrorType Type { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

    }

    public enum EErrorType
    {
        YakeenError = 1,
        LocalError = 2
    }
}