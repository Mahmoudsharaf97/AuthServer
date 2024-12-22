using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace Auth_Core
{
    [Table("Roles")]
    public class ApplicationRole<T> : IdentityRole
    {
        public string? CreateUser { get; set; }
        public string CreateUserName { get; set; }
        public string? UpdateUser { get; set; }
        public string UpdateUserName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
        public string DeleteUserId { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
