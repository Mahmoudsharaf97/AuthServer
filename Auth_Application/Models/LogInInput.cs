

namespace Auth_Application.Models
{
    public class LogInInput
    {
        public string?  Email { get; set; }
        public long?  NationalId { get; set; }
        public string Password { get; set; }
        public int LoginType { get; set; }

    }
}
