using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace Auth_Core
{
    [Table("Users")]
    public class ApplicationUser<T> : IdentityUser  
    {
        public override string UserName { get; set; }
         
        public string? ClientId { get; set; }       
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? LastSuccessLogin { set; get; }
        public bool IsDeleted { get; set; }
        public long NationalId { get; set; }
    }
}
