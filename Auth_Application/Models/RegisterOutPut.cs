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
    }
}
