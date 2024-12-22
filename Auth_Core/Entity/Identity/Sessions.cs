using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Auth_Core
{
    [Table("SessionStatus")]
        public class SessionStatus
        {
            [Key]
            public int Id { get; set; }
            public string? SessionId { get; set; }
            public string? UserId { get; set; }
            public string? UserIP { get; set; }
            public string? UserAgent { get; set; }
            public string? MacAddress { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string? DeviceName { get; set; }
        }

}
