using Auth_Application.Models;

namespace IdentityApplication.Models
{
    public class RegisterOutPut : Errors
    {
        public RegisterOutPut()
        {
            errors = new List<Error>();
        }
        public bool IsValidPhone { get; set; } = false;
        public bool IsValidDateOfBirth { get; set; } = false;
        public bool OtpSend { get; set; } = false;
        public int RegisterStatusCode { get; set; }
    }
}
