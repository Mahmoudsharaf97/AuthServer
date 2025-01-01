
namespace Auth_Application.Models
{

    public class Errors
    {
        public List<Error> errors { get; set; }
        public bool Succes { set; get; 
    }
    public class Error
    {
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
    public enum ErrorCode
    {
        success,
        EmptyInput,
        CaptchaError,
        EmailAlreadyExist

    }
}
